using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IInvoiceTemplateRepository: IRepositoryBaseExtended<InvoiceTemplateGetRequest,InvoiceTemplateAddRequest,InvoiceTemplateUpdateRequest, InvoiceTemplate>
    {
        private static Task<InvoiceTemplate> Get(int id) => null!;
    }
}
