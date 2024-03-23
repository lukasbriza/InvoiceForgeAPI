using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IInvoiceRepository: IRepositoryBaseExtended<InvoiceGetRequest, InvoiceAddRequestRepository, InvoiceUpdateRequest, Invoice>
    {
        private static Task<InvoiceItem?> Get(int id) => null!;
    }
}