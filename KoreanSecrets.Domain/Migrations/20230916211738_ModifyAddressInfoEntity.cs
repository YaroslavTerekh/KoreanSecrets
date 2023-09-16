using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class ModifyAddressInfoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlatNumber",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Addresses",
                newName: "Warehouse");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Warehouse",
                table: "Addresses",
                newName: "ZipCode");

            migrationBuilder.AddColumn<long>(
                name: "FlatNumber",
                table: "Addresses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
