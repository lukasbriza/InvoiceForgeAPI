using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IClientRepository: IRepositoryBaseExtended<ClientGetRequest, ClientAddRequest, ClientUpdateRequest>
    {
        private static Task<Client?> Get(int id) => null!;
    }
}
