using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceForgeApi.Migrations
{
    public partial class TablesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "InvoiceEntityCopy");

            migrationBuilder.AddColumn<bool>(
                name: "Outdated",
                table: "InvoiceUserAccountCopy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OriginClientId",
                table: "InvoiceEntityCopy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginContractorId",
                table: "InvoiceEntityCopy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Outdated",
                table: "InvoiceEntityCopy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Outdated",
                table: "InvoiceAddressCopy",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Outdated",
                table: "InvoiceUserAccountCopy");

            migrationBuilder.DropColumn(
                name: "OriginClientId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropColumn(
                name: "OriginContractorId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropColumn(
                name: "Outdated",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropColumn(
                name: "Outdated",
                table: "InvoiceAddressCopy");

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "InvoiceEntityCopy",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
