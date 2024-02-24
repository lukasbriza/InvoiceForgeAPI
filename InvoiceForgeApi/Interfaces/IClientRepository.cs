using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IClientRepository: IRepositoryBaseWithClientExtended<ClientGetRequest, ClientAddRequest, ClientUpdateRequest>
    {
        private static Task<Client?> Get(int id) => null!;
    }
}
