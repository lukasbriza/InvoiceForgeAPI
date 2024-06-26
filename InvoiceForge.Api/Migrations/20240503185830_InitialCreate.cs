﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceForge.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Shortcut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SWIFT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Shortcut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tariff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthenticationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceUserAccountCopy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outdated = table.Column<bool>(type: "bit", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    OriginId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceUserAccountCopy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceUserAccountCopy_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetNumber = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Address_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceAddressCopy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outdated = table.Column<bool>(type: "bit", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    OriginId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetNumber = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAddressCopy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceAddressCopy_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceAddressCopy_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TariffId = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItem_Tariff_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariff",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceItem_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Numbering",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberingTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberingPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Numbering", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Numbering_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccount_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAccount_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IN = table.Column<long>(type: "bigint", nullable: false),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Client_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contractor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IN = table.Column<long>(type: "bigint", nullable: false),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Www = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contractor_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contractor_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceEntityCopy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outdated = table.Column<bool>(type: "bit", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    OriginClientId = table.Column<int>(type: "int", nullable: true),
                    OriginContractorId = table.Column<int>(type: "int", nullable: true),
                    AddressCopyId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IN = table.Column<long>(type: "bigint", nullable: false),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Www = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceEntityCopy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceEntityCopy_InvoiceAddressCopy_AddressCopyId",
                        column: x => x.AddressCopyId,
                        principalTable: "InvoiceAddressCopy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceEntityCopy_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    NumberingId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ContractorId = table.Column<int>(type: "int", nullable: false),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceTemplate_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceTemplate_Contractor_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceTemplate_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTemplate_Numbering_NumberingId",
                        column: x => x.NumberingId,
                        principalTable: "Numbering",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceTemplate_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTemplate_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outdated = table.Column<bool>(type: "bit", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    NumberingId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    BasePriceTotal = table.Column<long>(type: "bigint", nullable: false),
                    VATTotal = table.Column<long>(type: "bigint", nullable: false),
                    TotalAll = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientCopyId = table.Column<int>(type: "int", nullable: false),
                    ContractorCopyId = table.Column<int>(type: "int", nullable: false),
                    UserAccountCopyId = table.Column<int>(type: "int", nullable: false),
                    Maturity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exposure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxableTransaction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_InvoiceTemplate_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "InvoiceTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoice_InvoiceUserAccountCopy_UserAccountCopyId",
                        column: x => x.UserAccountCopyId,
                        principalTable: "InvoiceUserAccountCopy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoice_User_Owner",
                        column: x => x.Owner,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceInvoiceEntityCopy",
                columns: table => new
                {
                    InvoiceEntityCopiesId = table.Column<int>(type: "int", nullable: false),
                    InvoicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceInvoiceEntityCopy", x => new { x.InvoiceEntityCopiesId, x.InvoicesId });
                    table.ForeignKey(
                        name: "FK_InvoiceInvoiceEntityCopy_Invoice_InvoicesId",
                        column: x => x.InvoicesId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceInvoiceEntityCopy_InvoiceEntityCopy_InvoiceEntityCopiesId",
                        column: x => x.InvoiceEntityCopiesId,
                        principalTable: "InvoiceEntityCopy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    InvoiceItemId = table.Column<int>(type: "int", nullable: false),
                    Units = table.Column<long>(type: "bigint", nullable: false),
                    PricePerUnit = table.Column<long>(type: "bigint", nullable: false),
                    BasePrice = table.Column<long>(type: "bigint", nullable: false),
                    VAT = table.Column<long>(type: "bigint", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceService_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceService_InvoiceItem_InvoiceItemId",
                        column: x => x.InvoiceItemId,
                        principalTable: "InvoiceItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CountryId",
                table: "Address",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_Owner",
                table: "Address",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_Client_AddressId",
                table: "Client",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Owner",
                table: "Client",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_Contractor_AddressId",
                table: "Contractor",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Contractor_Owner",
                table: "Contractor",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Owner",
                table: "Invoice",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_TemplateId",
                table: "Invoice",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_UserAccountCopyId",
                table: "Invoice",
                column: "UserAccountCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAddressCopy_CountryId",
                table: "InvoiceAddressCopy",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAddressCopy_UserId",
                table: "InvoiceAddressCopy",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceEntityCopy_AddressCopyId",
                table: "InvoiceEntityCopy",
                column: "AddressCopyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceEntityCopy_UserId",
                table: "InvoiceEntityCopy",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceInvoiceEntityCopy_InvoicesId",
                table: "InvoiceInvoiceEntityCopy",
                column: "InvoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_Owner",
                table: "InvoiceItem",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_TariffId",
                table: "InvoiceItem",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceService_InvoiceId",
                table: "InvoiceService",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceService_InvoiceItemId",
                table: "InvoiceService",
                column: "InvoiceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTemplate_ClientId",
                table: "InvoiceTemplate",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTemplate_ContractorId",
                table: "InvoiceTemplate",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTemplate_CurrencyId",
                table: "InvoiceTemplate",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTemplate_NumberingId",
                table: "InvoiceTemplate",
                column: "NumberingId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTemplate_Owner",
                table: "InvoiceTemplate",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTemplate_UserAccountId",
                table: "InvoiceTemplate",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceUserAccountCopy_BankId",
                table: "InvoiceUserAccountCopy",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Numbering_Owner",
                table: "Numbering",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_BankId",
                table: "UserAccount",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_Owner",
                table: "UserAccount",
                column: "Owner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceInvoiceEntityCopy");

            migrationBuilder.DropTable(
                name: "InvoiceService");

            migrationBuilder.DropTable(
                name: "InvoiceEntityCopy");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "InvoiceItem");

            migrationBuilder.DropTable(
                name: "InvoiceAddressCopy");

            migrationBuilder.DropTable(
                name: "InvoiceTemplate");

            migrationBuilder.DropTable(
                name: "InvoiceUserAccountCopy");

            migrationBuilder.DropTable(
                name: "Tariff");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Contractor");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Numbering");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
