

using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
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

                    invoice.InvoiceServices.ForEach(async service => {
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
                    });

                    if (invoiceServiceList.Count != invoice.InvoiceServices.Count) throw new ValidationError("Some invoice item id is invalid.");
                    var agregatedObject = invoiceServiceList
                        .Select(s => new {s.VAT, s.BasePrice, s.Total})
                        .Aggregate((a,b) => new {
                            VAT = a.VAT + b.VAT,
                            BasePrice = a.BasePrice + b.BasePrice,
                            Total = a.Total + b.Total
                        });
                    
                    ClientGetRequest? localCLient = await _repository.Client.GetById(isTemplate.ClientId);
                    ContractorGetRequest? localContractor = await _repository.Contractor.GetById(isTemplate.ContractorId);
                    UserAccountGetRequest? localUserAccount = await _repository.UserAccount.GetById(isTemplate.UserAccountId);

                    if (localCLient is null || localContractor is null || localUserAccount is null) throw new ValidationError("Unable to find local entities.");
                    
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

                        ClientLocal = localCLient,
                        ContractorLocal = localContractor,
                        UserAccountLocal = localUserAccount
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