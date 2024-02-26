using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IClientRepository: IRepositoryBaseWithClientExtended<ClientGetRequest, ClientAddRequest, ClientUpdateRequest, Client>
    {
        private static Task<Client?> Get(int id) => null!;
    }
}
