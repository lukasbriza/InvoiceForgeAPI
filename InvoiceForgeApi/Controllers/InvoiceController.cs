using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceServiceRepository _invoiceServiceRepository;
        private readonly IInvoiceTemplateRepository _invoiceTemplateRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IContractorRepository _contractorRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly INumberingRepository _numberingRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IRepositoryWrapper _repository;

        public InvoiceController(IRepositoryWrapper repository)
        {
            _invoiceServiceRepository = repository.InvoiceService;
            _invoiceRepository = repository.Invoice;
            _userRepository = repository.User;
            _invoiceTemplateRepository = repository.InvoiceTemplate;
            _invoiceItemRepository = repository.InvoiceItem;
            _numberingRepository = repository.Numbering;
            _contractorRepository = repository.Contractor;
            _clientRepository = repository.Client;
            _userAccountRepository = repository.UserAccount;
            _repository = repository;
        }

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<InvoiceGetRequest>?> GetAllInvoices(int userId)
        {
            return await _invoiceRepository.GetAll(userId);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<InvoiceGetRequest>?> GetPlainAllInvoices(int userId)
        {
            return await _invoiceRepository.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{invoiceId}")]
        public async Task<InvoiceGetRequest?> GetInvoiceById(int invoiceId)
        {
            return await _invoiceRepository.GetById(invoiceId);
        }
        [HttpGet]
        [Route("plain/{invoiceId}")]
        public async Task<InvoiceGetRequest?> GetPlainInvoiceById (int invoiceId)
        {
            return await _invoiceRepository.GetById(invoiceId, true);
        }
        [HttpPost]
        [Route("generate/{userId}")]
        public async Task<bool> GenerateInvoice(int userId, InvoiceAddRequest invoice)
        {
            var user = await _userRepository.GetById(userId);
            if (user is null) throw new ValidationError("Provided userId is invalid.");

            var template = await _invoiceTemplateRepository.GetById(invoice.TemplateId);
            if (template is null) throw new ValidationError("Provided templateId is invalid.");
            if (template.Owner != userId) throw new ValidationError("Template is not in your possession.");


            var invoiceNumberResponse = await _numberingRepository.GenerateInvoiceNumber(template.NumberingId);
            if (invoiceNumberResponse is null) throw new ValidationError("generating invoice number failed.");

            var invoiceServicesList = new List<InvoiceServiceExtendedAddRequest>();

            async void ValidateInvoiceService(InvoiceServiceAddRequest service)
            {
                var invoiceItem = await _invoiceItemRepository.GetById(service.ItemId);
                if (invoiceItem is not null) {
                    var BasePrice = service.PricePerUnit * service.Units;
                    var VAT = (long)invoiceItem.Tariff!.Value/100 * BasePrice;

                    invoiceServicesList.Add(new InvoiceServiceExtendedAddRequest
                        {
                            VAT = VAT,
                            BasePrice = BasePrice,
                            Total = VAT + BasePrice,
                            PricePerUnit = service.PricePerUnit,
                            Units = service.Units,
                            ItemId = service.ItemId,
                        }
                    );
                }
            };

            invoice.InvoiceServices.ForEach(ValidateInvoiceService);
            if (invoiceServicesList.Count != invoice.InvoiceServices.Count) throw new ValidationError("Some invoice item id is invalid.");

            var agregatedObject = invoiceServicesList.Select(s => new {s.VAT, s.BasePrice, s.Total}).Aggregate((a,b) => new {
                VAT = a.VAT + b.VAT,
                BasePrice = a.BasePrice + b.BasePrice,
                Total = a.Total + b.Total
            });
            
            //GET LOCAL COPY OF TEMPLATE OBJECTS
            var localCLient = await _clientRepository.GetById(template.ClientId);
            var localContractor = await _contractorRepository.GetById(template.ContractorId);
            var localUserAccount = await _userAccountRepository.GetById(template.UserAccountId);

            if (localCLient is null || localContractor is null || localUserAccount is null)
            {
                _repository.DetachChanges();
                throw new ValidationError("Unable to find local entities.");
            }

            var newInvoiceObject = new InvoiceAddRequestRepository
            {
                InvoiceNumber = invoiceNumberResponse.invoiceNumber,
                OrderNumber = invoiceNumberResponse.invoiceOrder,
                TemplateId = template.Id,
                NumberingId = template.NumberingId,
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

            var addInvoiceId = await _invoiceRepository.Add(userId, newInvoiceObject);
            if (addInvoiceId is not null) {
                var addServiceLists = await _invoiceServiceRepository.Add((int)addInvoiceId, invoiceServicesList);
            }

            _repository.DetachChanges();
            return false;
        }
        [HttpPut]
        [Route("{invoiceId}")]
        public async Task<bool> UpdateInvoice(int invoiceId, InvoiceUpdateRequest invoice)
        {
            if (invoice is null) throw new ValidationError("Invoice is not provided.");

            var user = await _userRepository.GetById(invoice.Owner);
            if (user is null) throw new ValidationError("Provided userId is invalid.");

            var isOwnerOfInvoice = _invoiceRepository.GetByCondition(i => i.Owner == user.Id);
            if (isOwnerOfInvoice.Id != invoiceId) throw new ValidationError("Provided invoice is not in your possession.");

            var updateInvoice = await _invoiceRepository.Update(invoiceId, invoice);
            if (updateInvoice)
            {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            }
            return updateInvoice;
        }
        [HttpDelete]
        [Route("{invoiceId}")]
        public async Task<bool> DeleteInvoice(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetById(invoiceId, true);
            if (invoice is null) throw new ValidationError("There is no invoice with that id.");

            var services = await _invoiceServiceRepository.GetByCondition(s => s.InvoiceId == invoiceId);
            services?.ForEach(async s => {
                var deleteService = await _invoiceServiceRepository.Delete(s.Id);
                if (deleteService == false)
                {
                    _repository.DetachChanges();
                    throw new ValidationError("Removing invoice service failed.");
                }
            });
            
            var deleteInvoice = await _invoiceRepository.Delete(invoiceId);
            if (deleteInvoice)
            {
                await _repository.Save();
            } else {
                _repository.DetachChanges();
            }
            return deleteInvoice;
        }
    }
}