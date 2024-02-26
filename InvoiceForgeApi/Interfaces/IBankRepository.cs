using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model.CodeLists;

namespace InvoiceForgeApi.Interfaces
{
    public interface IBankRepository: IRepositoryBase<BankGetRequest, BankAddRequest, BankUpdateRequest, Bank> {}
}
