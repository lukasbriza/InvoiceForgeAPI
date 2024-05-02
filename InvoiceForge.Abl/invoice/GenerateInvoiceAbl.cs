using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;


namespace InvoiceForgeApi.Abl.invoice
{
    public class GenerateInvoiceAbl: AblBase
    {
        public GenerateInvoiceAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userId, InvoiceAddRequest invoice)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    User isUser = await IsInDatabase<User>(userId, "Invalid user Id.");
                    InvoiceTemplate isTemplate = await IsInDatabase<InvoiceTemplate>(invoice.TemplateId, "Invalid template Id.");
                    if (isTemplate.Owner != userId) throw new ValidationError("Template is not in your possession.");

                    GenerateInvoiceNumber? invoiceNumber = await _repository.Numbering.GenerateInvoiceNumber(isTemplate.NumberingId);
                    if (invoiceNumber is null) throw new ValidationError("Generating invoice number failed.");

                    var invoiceServiceList = new List<InvoiceServiceExtendedAddRequest>();

                    async Task addService(InvoiceServiceAddRequest service)
                    {
                        var invoiceItem = await _repository.InvoiceItem.GetById(service.ItemId);
                        if (invoiceItem is not null)
                        {
                            var basePrice = service.PricePerUnit * service.Units;
                            var VAT = (long)invoiceItem.Tariff!.Value/100 * basePrice;

                            invoiceServiceList.Add(new InvoiceServiceExtendedAddRequest
                                {
                                    VAT = VAT,
                                    BasePrice = basePrice,
                                    Total = VAT + basePrice,
                                    PricePerUnit = service.PricePerUnit,
                                    Units = service.Units,
                                    ItemId = service.ItemId
                                }
                            );
                        }
                    }

                    invoice.InvoiceServices.ForEach(service => {
                        var task = addService(service);
                        task.Wait();
                    });

                    if (invoiceServiceList.Count != invoice.InvoiceServices.Count) throw new ValidationError("Some invoice item id is invalid.");
                    var agregatedObject = invoiceServiceList
                        .Select(s => new {s.VAT, s.BasePrice, s.Total})
                        .Aggregate((a,b) => new {
                            VAT = a.VAT + b.VAT,
                            BasePrice = a.BasePrice + b.BasePrice,
                            Total = a.Total + b.Total
                        });
                    
                    CurrencyGetRequest? templateCurrency = await _repository.CodeLists.GetCurrencyById(isTemplate.CurrencyId);
                    ClientGetRequest? invoiceClient = await _repository.Client.GetById(isTemplate.ClientId, false);
                    ContractorGetRequest? invoiceContractor = await _repository.Contractor.GetById(isTemplate.ContractorId, false);
                    UserAccountGetRequest? invoiceUserAccount = await _repository.UserAccount.GetById(isTemplate.UserAccountId, true);
                    
                    if (
                        invoiceClient is null || 
                        invoiceContractor is null || 
                        invoiceUserAccount is null || 
                        templateCurrency is null
                    ) throw new ValidationError("Unable to find local entities.");
                     
                    List<InvoiceEntityCopy>? isDuplicitClientInEntities = await _repository.InvoiceEntityCopy.GetByCondition(e => 
                        e.Outdated == false &&
                        e.Owner == invoiceClient.Owner &&
                        e.Type == invoiceClient.Type &&
                        e.Name == invoiceClient.Name &&
                        e.IN == invoiceClient.IN &&
                        e.TIN == invoiceClient.TIN &&
                        e.Email == invoiceClient.Email &&
                        e.Mobil == invoiceClient.Mobil &&
                        e.Tel == invoiceClient.Tel &&

                        e.AddressCopy.Outdated == false &&
                        e.AddressCopy.CountryId == invoiceClient.Address!.CountryId &&
                        e.AddressCopy.Street == invoiceClient.Address.Street &&
                        e.AddressCopy.StreetNumber == invoiceClient.Address.StreetNumber && 
                        e.AddressCopy.City == invoiceClient.Address.City &&
                        e.AddressCopy.PostalCode == invoiceClient.Address.PostalCode
                    );

                    List<InvoiceEntityCopy>? isDuplicitContractorInEntities = await _repository.InvoiceEntityCopy.GetByCondition(e =>
                        e.Outdated == false &&
                        e.Owner == invoiceContractor.Owner &&
                        e.Type == invoiceContractor.Type &&
                        e.Name == invoiceContractor.Name &&
                        e.IN == invoiceContractor.IN &&
                        e.TIN == invoiceContractor.TIN &&
                        e.Email == invoiceContractor.Email &&
                        e.Mobil == invoiceContractor.Mobil &&
                        e.Tel == invoiceContractor.Tel &&
                        e.Www == invoiceContractor.Www &&

                        e.AddressCopy.Outdated == false &&
                        e.AddressCopy.CountryId == invoiceContractor.Address!.CountryId &&
                        e.AddressCopy.Street == invoiceContractor.Address.Street &&
                        e.AddressCopy.StreetNumber == invoiceContractor.Address.StreetNumber && 
                        e.AddressCopy.City == invoiceContractor.Address.City &&
                        e.AddressCopy.PostalCode == invoiceContractor.Address.PostalCode
                    );

                    List<InvoiceUserAccountCopy>? isDuplicitUserAccountCopy = await _repository.InvoiceUserAccountCopy.GetByCondition(a =>
                        a.Outdated == false &&
                        a.Owner == invoiceUserAccount.Owner &&
                        a.BankId == invoiceUserAccount.BankId &&
                        a.AccountNumber == invoiceUserAccount.AccountNumber &&
                        a.IBAN == invoiceUserAccount.IBAN
                    );

                    int? invoiceClientCopyId = null;
                    int? invoiceContractorCopyId = null;
                    int? invoiceUserAccountCopyId = null;

                    if (isDuplicitClientInEntities is null || isDuplicitClientInEntities.Count == 0)
                    {
                        //ADD CLIENT AND CLIENT ADDRESS COPY
                        var address = new InvoiceAddressCopyAddRequest
                        {
                            OriginId = invoiceClient.Address!.Id,
                            CountryId = invoiceClient.Address.CountryId,
                            Street = invoiceClient.Address.Street,
                            StreetNumber = invoiceClient.Address.StreetNumber,
                            City = invoiceClient.Address.City,
                            PostalCode = invoiceClient.Address.PostalCode
                        };
                        int? addressId = await _repository.InvoiceAddressCopy.Add(userId, address);
                        if (addressId is null) throw new ValidationError("Address copy insertion failed.");
                        
                        var entity = new InvoiceEntityCopyAddRequest(invoiceClient, addressId);
                        invoiceClientCopyId = await _repository.InvoiceEntityCopy.Add(userId, entity);
                    } else if (isDuplicitClientInEntities.Count == 1) invoiceClientCopyId = isDuplicitClientInEntities[0].Id;
                    else throw new ValidationError("There is more than one local client.");
                    
                    if (isDuplicitContractorInEntities is null || isDuplicitContractorInEntities.Count == 0)
                    {
                        //ADD CONTRACTOR AND CONTRACTOR ADDRESS COPY
                        var address = new InvoiceAddressCopyAddRequest
                        {
                            OriginId = invoiceContractor.Address!.Id,
                            CountryId = invoiceContractor.Address.CountryId,
                            Street = invoiceContractor.Address.Street,
                            StreetNumber = invoiceContractor.Address.StreetNumber,
                            City = invoiceContractor.Address.City,
                            PostalCode = invoiceContractor.Address.PostalCode
                        };
                        int? addressId = await _repository.InvoiceAddressCopy.Add(userId, address);
                        if (addressId is null) throw new ValidationError("Address copy insertion failed.");

                        var entity = new InvoiceEntityCopyAddRequest(invoiceContractor, addressId);
                        invoiceContractorCopyId = await _repository.InvoiceEntityCopy.Add(userId, entity);
                    } else if (isDuplicitContractorInEntities.Count == 1) invoiceContractorCopyId = isDuplicitContractorInEntities[0].Id;
                    else throw new ValidationError("There is more than one local contractor.");

                    if (isDuplicitUserAccountCopy is null || isDuplicitUserAccountCopy.Count == 0)
                    {
                        //ADD USER ACCOUNT COPY
                        var entity = new InvoiceUserAccountCopyAddRequest(invoiceUserAccount);
                        invoiceUserAccountCopyId = await _repository.InvoiceUserAccountCopy.Add(userId, entity);
                    } else if (isDuplicitUserAccountCopy.Count == 1) invoiceUserAccountCopyId = isDuplicitUserAccountCopy[0].Id;
                    else throw new ValidationError("There is more than one local user account.");

                    if (invoiceClientCopyId == null || invoiceContractorCopyId == null || invoiceUserAccountCopyId == null) throw new ValidationError("Unasigned one of id variables.");
                    
                    var newInvoiceObject = new InvoiceAddRequestRepository
                    {
                        InvoiceNumber = invoiceNumber.invoiceNumber,
                        OrderNumber = invoiceNumber.invoiceOrder,
                        TemplateId = isTemplate.Id,
                        NumberingId = isTemplate.NumberingId,
                        BasePriceTotal = agregatedObject.BasePrice,
                        VATTotal = agregatedObject.VAT,
                        TotalAll = agregatedObject.Total,
                        Maturity = invoice.Maturity,
                        Exposure = invoice.Exposure,
                        TaxableTransaction = invoice.TaxableTransaction,
                        Created = DateTime.UtcNow,
                        
                        ClientCopyId = (int)invoiceClientCopyId,
                        ContractorCopyId = (int)invoiceContractorCopyId,
                        UserAccountCopyId = (int)invoiceUserAccountCopyId,

                        Currency = templateCurrency.Value
                    };

                    int? addInvoiceId = await _repository.Invoice.Add(userId, newInvoiceObject);
                    bool saveCondition = addInvoiceId is not null;
                    
                    if (addInvoiceId is not null) {
                        var addServiceLists = await _repository.InvoiceService.Add((int)addInvoiceId, invoiceServiceList);
                        if (!addServiceLists) throw new ValidationError("Add services list failed.");
                    }

                    await SaveResult(saveCondition, transaction);
                    return saveCondition;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}