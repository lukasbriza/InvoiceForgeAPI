using InvoiceForgeApi.Model;
using InvoiceForgeApi.Model.CodeLists;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Data
{
    public class InvoiceForgeDatabaseContext : DbContext
    {
        public InvoiceForgeDatabaseContext(DbContextOptions<InvoiceForgeDatabaseContext> options): base(options) {}

        public DbSet<Bank> Bank { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Client> Client {  get; set; }
        public DbSet<Contractor> Contractor { get; set; }
        public DbSet<InvoiceTemplate> InvoiceTemplate { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
    }
}
