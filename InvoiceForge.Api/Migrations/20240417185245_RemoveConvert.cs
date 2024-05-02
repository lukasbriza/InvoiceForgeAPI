using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceForgeApi.Migrations
{
    public partial class RemoveConvert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientLocal",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ContractorLocal",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "UserAccountLocal",
                table: "Invoice");

            migrationBuilder.RenameColumn(
                name: "ContractorName",
                table: "Contractor",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ClientType",
                table: "Contractor",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "Client",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "UserAccount",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientCopyId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractorCopyId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserAccountCopyId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Client",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "InvoiceAddressCopy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetNumber = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<int>(type: "int", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "InvoiceUserAccountCopy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "InvoiceEntityCopy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IN = table.Column<long>(type: "bigint", nullable: true),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Www = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressCopyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceEntityCopy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceEntityCopy_InvoiceAddressCopy_AddressCopyId",
                        column: x => x.AddressCopyId,
                        principalTable: "InvoiceAddressCopy",
                        principalColumn: "Id");
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

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_UserAccountCopyId",
                table: "Invoice",
                column: "UserAccountCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAddressCopy_CountryId",
                table: "InvoiceAddressCopy",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceEntityCopy_AddressCopyId",
                table: "InvoiceEntityCopy",
                column: "AddressCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceInvoiceEntityCopy_InvoicesId",
                table: "InvoiceInvoiceEntityCopy",
                column: "InvoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceUserAccountCopy_BankId",
                table: "InvoiceUserAccountCopy",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_InvoiceUserAccountCopy_UserAccountCopyId",
                table: "Invoice",
                column: "UserAccountCopyId",
                principalTable: "InvoiceUserAccountCopy",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_InvoiceUserAccountCopy_UserAccountCopyId",
                table: "Invoice");

            migrationBuilder.DropTable(
                name: "InvoiceInvoiceEntityCopy");

            migrationBuilder.DropTable(
                name: "InvoiceUserAccountCopy");

            migrationBuilder.DropTable(
                name: "InvoiceEntityCopy");

            migrationBuilder.DropTable(
                name: "InvoiceAddressCopy");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_UserAccountCopyId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ClientCopyId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ContractorCopyId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "UserAccountCopyId",
                table: "Invoice");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Contractor",
                newName: "ClientType");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Contractor",
                newName: "ContractorName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Client",
                newName: "ClientName");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "UserAccount",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ClientLocal",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContractorLocal",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserAccountLocal",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Client",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
