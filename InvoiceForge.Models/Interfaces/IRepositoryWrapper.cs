using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InvoiceForgeApi.Models.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserAccountRepository UserAccount { get; }
        IClientRepository Client { get; }
        ICodeListsRepository CodeLists { get; }
        IAddressRepository Address { get; }
        IInvoiceTemplateRepository InvoiceTemplate { get; }
        IContractorRepository Contractor { get; }
        IInvoiceItemRepository InvoiceItem { get; }
        IInvoiceServiceRepository InvoiceService { get; }
        IInvoiceRepository Invoice { get; }
        IInvoiceEntityCopyRepository InvoiceEntityCopy { get; }
        IInvoiceAddressCopyRepository InvoiceAddressCopy { get; }
        IInvoiceUserAccountCopyRepository InvoiceUserAccountCopy { get; }
        INumberingRepository Numbering { get; }
        
        Task<DbSet<TEntity>?> GetSet<TEntity>() where TEntity: class;
        Task Save();
        void DetachChanges();
        Task DisposeAsync();
        Task<IDbContextTransaction> BeginTransaction();
    }
}