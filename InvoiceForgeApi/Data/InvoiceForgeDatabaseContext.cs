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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<UserAccount>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<InvoiceTemplate>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Contractor>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Client>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Address>()
                .Property (u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Country>()
                .Property (u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Bank>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            //Address on delete behavior
            builder.Entity<Address>().HasOne(a => a.Country).WithMany(c => c.Addresses).OnDelete(DeleteBehavior.ClientNoAction);

            //Client ond delete behavior
            builder.Entity<Client>().HasOne(a => a.Address).WithMany(a => a.Clients).OnDelete(DeleteBehavior.ClientNoAction);

            //Contractor on delete behavior
            builder.Entity<Contractor>().HasOne(c => c.Address).WithMany(a => a.Contractors).OnDelete(DeleteBehavior.ClientNoAction);

            //UserAccount on delete behavior
            builder.Entity<UserAccount>().HasOne(a => a.Bank).WithMany(b => b.UserAccounts).OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
