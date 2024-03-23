using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IInvoiceItemRepository: IRepositoryBaseExtended<InvoiceItemGetRequest, InvoiceItemAddRequest,InvoiceItemUpdateRequest, InvoiceItem>
    {
        private static Task<InvoiceItem?> Get(int id) => null!;
    }
}