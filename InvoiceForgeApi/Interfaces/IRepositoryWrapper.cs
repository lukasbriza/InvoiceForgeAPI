namespace InvoiceForgeApi.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserAccountRepository UserAccount { get; }
        IClientRepository Client { get; }
        ICodeListsRepository CodeLists { get; }
        IAddressRepository Address { get; }
        Task Save();
    }
}