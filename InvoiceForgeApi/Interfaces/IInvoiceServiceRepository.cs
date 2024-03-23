using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IInvoiceServiceRepository: IRepositoryBaseExtended<InvoiceServiceGetRequest, InvoiceServiceExtendedAddRequest,InvoiceServiceUpdateRequest, InvoiceService>
    {
        public Task<bool> Add(int InvoiceId, List<InvoiceServiceExtendedAddRequest> invoiceServices);
        private static Task<InvoiceItem?> Get(int id) => null!;
    }
}