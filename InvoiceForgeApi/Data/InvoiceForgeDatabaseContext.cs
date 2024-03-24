using InvoiceForgeApi.Data.Enum;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;
using InvoiceForgeApi.Model.CodeLists;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public DbSet<Tariff> Tariff { get; set; } = null!;
        public DbSet<Currency> Currency { get; set; } = null!;
        public DbSet<Invoice> Invoice { get; set; } = null!;
        public DbSet<InvoiceItem> InvoiceItem { get; set; } = null!;
        public DbSet<InvoiceService> InvoiceService { get; set; } = null!;
        public DbSet<Numbering> Numbering { get; set; } = null!;

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
            builder.Entity<Tariff>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Currency>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Invoice>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<InvoiceService>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<InvoiceItem>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Numbering>()
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

            //Invoice on delete behavior
            builder.Entity<Invoice>().HasOne(a => a.InvoiceTemplate).WithMany(b => b.Invoices).OnDelete(DeleteBehavior.ClientNoAction);

            //InvoiceItem on delete behavior
            builder.Entity<InvoiceItem>().HasOne(a => a.Tariff).WithMany(b => b.InvoiceItems).OnDelete(DeleteBehavior.ClientNoAction);
        
            //InvoiceService on delete behavior
            builder.Entity<InvoiceService>().HasOne(a => a.InvoiceItem).WithMany(b => b.InvoiceServices).OnDelete(DeleteBehavior.ClientNoAction);

            //InvoiceTemplate on delete behavior
            builder.Entity<InvoiceTemplate>().HasOne(a => a.Client).WithMany(b => b.InvoiceTemplates).OnDelete(DeleteBehavior.ClientNoAction);
            builder.Entity<InvoiceTemplate>().HasOne(a => a.Contractor).WithMany(b => b.InvoiceTemplates).OnDelete(DeleteBehavior.ClientNoAction);
            builder.Entity<InvoiceTemplate>().HasOne(a => a.UserAccount).WithMany(b => b.InvoiceTemplates).OnDelete(DeleteBehavior.ClientNoAction);
            builder.Entity<InvoiceTemplate>().HasOne(a => a.Numbering).WithMany(b => b.InvoiceTemplates).OnDelete(DeleteBehavior.ClientNoAction);

            //Numbering NumberingTemplate serializer setup
            builder.Entity<Numbering>(e => {
                e.Property(t => t.NumberingTemplate)
                    .HasConversion(
                        c => JsonConvert.SerializeObject(c), 
                        c => JsonConvert.DeserializeObject<List<NumberingVariable>>(c)
                    );
            });

            //Invoice UserAccount, Client, Contractor serializer setup
            builder.Entity<Invoice>(e => {
                e.Property(i => i.ClientLocal)
                    .HasConversion(
                        c => JsonConvert.SerializeObject(c),
                        c => JsonConvert.DeserializeObject<ClientGetRequest>(c)
                    );
            });
            builder.Entity<Invoice>(e => {
                e.Property(i => i.ContractorLocal)
                    .HasConversion(
                        c => JsonConvert.SerializeObject(c),
                        c => JsonConvert.DeserializeObject<ContractorGetRequest>(c)
                    );
            });
            builder.Entity<Invoice>(e => {
                e.Property(i => i.UserAccountLocal)
                    .HasConversion(
                        c => JsonConvert.SerializeObject(c),
                        c => JsonConvert.DeserializeObject<UserAccountGetRequest>(c)
                    );
            });
        }
    }
}
