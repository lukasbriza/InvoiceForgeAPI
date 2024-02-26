using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IContractorRepository: IRepositoryBaseWithClientExtended<ContractorGetRequest, ContractorAddRequest,ContractorUpdateRequest, Contractor>
    {
        private static Task<Contractor?> Get(int id) => null!;
    }
}
