using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceForgeApi.Migrations
{
    public partial class CopyTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceEntityCopy_InvoiceAddressCopy_AddressCopyId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceEntityCopy_AddressCopyId",
                table: "InvoiceEntityCopy");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "InvoiceEntityCopy",
                newName: "Owner");

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "InvoiceUserAccountCopy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Owner",
                table: "InvoiceUserAccountCopy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "InvoiceEntityCopy",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IN",
                table: "InvoiceEntityCopy",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressCopyId",
                table: "InvoiceEntityCopy",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "InvoiceEntityCopy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "InvoiceEntityCopy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "InvoiceAddressCopy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Owner",
                table: "InvoiceAddressCopy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "InvoiceAddressCopy",
                type: "int",
                nullable: true);

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
                name: "IX_InvoiceAddressCopy_UserId",
                table: "InvoiceAddressCopy",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceAddressCopy_User_UserId",
                table: "InvoiceAddressCopy",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceEntityCopy_InvoiceAddressCopy_AddressCopyId",
                table: "InvoiceEntityCopy",
                column: "AddressCopyId",
                principalTable: "InvoiceAddressCopy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceEntityCopy_User_UserId",
                table: "InvoiceEntityCopy",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceAddressCopy_User_UserId",
                table: "InvoiceAddressCopy");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceEntityCopy_InvoiceAddressCopy_AddressCopyId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceEntityCopy_User_UserId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceEntityCopy_AddressCopyId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceEntityCopy_UserId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceAddressCopy_UserId",
                table: "InvoiceAddressCopy");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "InvoiceUserAccountCopy");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "InvoiceUserAccountCopy");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InvoiceEntityCopy");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "InvoiceAddressCopy");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "InvoiceAddressCopy");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InvoiceAddressCopy");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "InvoiceEntityCopy",
                newName: "AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "InvoiceEntityCopy",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "IN",
                table: "InvoiceEntityCopy",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "AddressCopyId",
                table: "InvoiceEntityCopy",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceEntityCopy_AddressCopyId",
                table: "InvoiceEntityCopy",
                column: "AddressCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceEntityCopy_InvoiceAddressCopy_AddressCopyId",
                table: "InvoiceEntityCopy",
                column: "AddressCopyId",
                principalTable: "InvoiceAddressCopy",
                principalColumn: "Id");
        }
    }
}
