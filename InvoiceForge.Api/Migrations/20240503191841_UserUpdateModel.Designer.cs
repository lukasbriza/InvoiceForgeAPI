﻿// <auto-generated />
using System;
using InvoiceForgeApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InvoiceForge.Api.Migrations
{
    [DbContext(typeof(InvoiceForgeDatabaseContext))]
    [Migration("20240503191841_UserUpdateModel")]
    partial class UserUpdateModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("InvoiceForgeApi.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<int>("PostalCode")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StreetNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("Owner");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("SWIFT")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Shortcut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("IN")
                        .HasColumnType("bigint");

                    b.Property<string>("Mobil")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<string>("TIN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("Owner");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Contractor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("IN")
                        .HasColumnType("bigint");

                    b.Property<string>("Mobil")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<string>("TIN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Www")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("Owner");

                    b.ToTable("Contractor");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Shortcut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long>("BasePriceTotal")
                        .HasColumnType("bigint");

                    b.Property<int>("ClientCopyId")
                        .HasColumnType("int");

                    b.Property<int>("ContractorCopyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Exposure")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Maturity")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberingId")
                        .HasColumnType("int");

                    b.Property<long>("OrderNumber")
                        .HasColumnType("bigint");

                    b.Property<bool>("Outdated")
                        .HasColumnType("bit");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<DateTime>("TaxableTransaction")
                        .HasColumnType("datetime2");

                    b.Property<int>("TemplateId")
                        .HasColumnType("int");

                    b.Property<long>("TotalAll")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserAccountCopyId")
                        .HasColumnType("int");

                    b.Property<long>("VATTotal")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.HasIndex("TemplateId");

                    b.HasIndex("UserAccountCopyId");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceAddressCopy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<int>("OriginId")
                        .HasColumnType("int");

                    b.Property<bool>("Outdated")
                        .HasColumnType("bit");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<int>("PostalCode")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StreetNumber")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("UserId");

                    b.ToTable("InvoiceAddressCopy");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceEntityCopy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressCopyId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("IN")
                        .HasColumnType("bigint");

                    b.Property<string>("Mobil")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OriginClientId")
                        .HasColumnType("int");

                    b.Property<int?>("OriginContractorId")
                        .HasColumnType("int");

                    b.Property<bool>("Outdated")
                        .HasColumnType("bit");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<string>("TIN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Www")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressCopyId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("InvoiceEntityCopy");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<int>("TariffId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.HasIndex("TariffId");

                    b.ToTable("InvoiceItem");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long>("BasePrice")
                        .HasColumnType("bigint");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("InvoiceItemId")
                        .HasColumnType("int");

                    b.Property<long>("PricePerUnit")
                        .HasColumnType("bigint");

                    b.Property<long>("Total")
                        .HasColumnType("bigint");

                    b.Property<long>("Units")
                        .HasColumnType("bigint");

                    b.Property<long>("VAT")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("InvoiceItemId");

                    b.ToTable("InvoiceService");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("ContractorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("NumberingId")
                        .HasColumnType("int");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<string>("TemplateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserAccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ContractorId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("NumberingId");

                    b.HasIndex("Owner");

                    b.HasIndex("UserAccountId");

                    b.ToTable("InvoiceTemplate");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceUserAccountCopy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<string>("IBAN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OriginId")
                        .HasColumnType("int");

                    b.Property<bool>("Outdated")
                        .HasColumnType("bit");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("InvoiceUserAccountCopy");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Numbering", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("NumberingPrefix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumberingTemplate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.ToTable("Numbering");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Tariff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tariff");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthenticationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.UserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("IBAN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Owner")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.HasIndex("Owner");

                    b.ToTable("UserAccount");
                });

            modelBuilder.Entity("InvoiceInvoiceEntityCopy", b =>
                {
                    b.Property<int>("InvoiceEntityCopiesId")
                        .HasColumnType("int");

                    b.Property<int>("InvoicesId")
                        .HasColumnType("int");

                    b.HasKey("InvoiceEntityCopiesId", "InvoicesId");

                    b.HasIndex("InvoicesId");

                    b.ToTable("InvoiceInvoiceEntityCopy");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Address", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Country", "Country")
                        .WithMany("Addresses")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Client", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Address", "Address")
                        .WithMany("Clients")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany("Clients")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Contractor", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Address", "Address")
                        .WithMany("Contractors")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany("Contractors")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Invoice", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.InvoiceTemplate", "InvoiceTemplate")
                        .WithMany("Invoices")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.InvoiceUserAccountCopy", "InvoiceUserAccountCopy")
                        .WithMany("Invoices")
                        .HasForeignKey("UserAccountCopyId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("InvoiceTemplate");

                    b.Navigation("InvoiceUserAccountCopy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceAddressCopy", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Country", "Country")
                        .WithMany("AddressCopies")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.User", null)
                        .WithMany("InvoiceAddressCopies")
                        .HasForeignKey("UserId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceEntityCopy", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.InvoiceAddressCopy", "AddressCopy")
                        .WithOne("InvoiceEntityCopies")
                        .HasForeignKey("InvoiceForgeApi.Models.InvoiceEntityCopy", "AddressCopyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.User", null)
                        .WithMany("InvoiceEntityCopies")
                        .HasForeignKey("UserId");

                    b.Navigation("AddressCopy");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceItem", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.Tariff", "Tariff")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("TariffId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Tariff");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceService", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Invoice", "Invoice")
                        .WithMany("InvoiceServices")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.InvoiceItem", "InvoiceItem")
                        .WithMany("InvoiceServices")
                        .HasForeignKey("InvoiceItemId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("InvoiceItem");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceTemplate", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Client", "Client")
                        .WithMany("InvoiceTemplates")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.Contractor", "Contractor")
                        .WithMany("InvoiceTemplates")
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.Currency", "Currency")
                        .WithMany("InvoiceTemplates")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.Numbering", "Numbering")
                        .WithMany("InvoiceTemplates")
                        .HasForeignKey("NumberingId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany("InvoiceTemplates")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.UserAccount", "UserAccount")
                        .WithMany("InvoiceTemplates")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Contractor");

                    b.Navigation("Currency");

                    b.Navigation("Numbering");

                    b.Navigation("User");

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceUserAccountCopy", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Bank", "Bank")
                        .WithMany("UserAccountCopies")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Numbering", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany("Numberings")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.UserAccount", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.Bank", "Bank")
                        .WithMany("UserAccounts")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.User", "User")
                        .WithMany("UserAccounts")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InvoiceInvoiceEntityCopy", b =>
                {
                    b.HasOne("InvoiceForgeApi.Models.InvoiceEntityCopy", null)
                        .WithMany()
                        .HasForeignKey("InvoiceEntityCopiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceForgeApi.Models.Invoice", null)
                        .WithMany()
                        .HasForeignKey("InvoicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Address", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("Contractors");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Bank", b =>
                {
                    b.Navigation("UserAccountCopies");

                    b.Navigation("UserAccounts");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Client", b =>
                {
                    b.Navigation("InvoiceTemplates");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Contractor", b =>
                {
                    b.Navigation("InvoiceTemplates");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Country", b =>
                {
                    b.Navigation("AddressCopies");

                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Currency", b =>
                {
                    b.Navigation("InvoiceTemplates");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Invoice", b =>
                {
                    b.Navigation("InvoiceServices");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceAddressCopy", b =>
                {
                    b.Navigation("InvoiceEntityCopies")
                        .IsRequired();
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceItem", b =>
                {
                    b.Navigation("InvoiceServices");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceTemplate", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.InvoiceUserAccountCopy", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Numbering", b =>
                {
                    b.Navigation("InvoiceTemplates");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.Tariff", b =>
                {
                    b.Navigation("InvoiceItems");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.User", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Clients");

                    b.Navigation("Contractors");

                    b.Navigation("InvoiceAddressCopies");

                    b.Navigation("InvoiceEntityCopies");

                    b.Navigation("InvoiceItems");

                    b.Navigation("InvoiceTemplates");

                    b.Navigation("Numberings");

                    b.Navigation("UserAccounts");
                });

            modelBuilder.Entity("InvoiceForgeApi.Models.UserAccount", b =>
                {
                    b.Navigation("InvoiceTemplates");
                });
#pragma warning restore 612, 618
        }
    }
}
