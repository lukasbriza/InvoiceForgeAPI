using InvoiceForgeApi.Model;
using InvoiceForgeApi.Model.CodeLists;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Data
{
    public class InvoiceForgeDatabaseContext : DbContext
    {
        public InvoiceForgeDatabaseContext(DbContextOptions<InvoiceForgeDatabaseContext> options): base(options) {}

        public DbSet<Bank> Bank { get; set; } = null!;
        public DbSet<Country> Country { get; set; } = null!;
        public DbSet<Address> Address { get; set; } = null!;
        public DbSet<Client> Client {  get; set; } = null!;
        public DbSet<Contractor> Contractor { get; set; } = null!;
        public DbSet<InvoiceTemplate> InvoiceTemplate { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<UserAccount> UserAccount { get; set; } = null!;

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
