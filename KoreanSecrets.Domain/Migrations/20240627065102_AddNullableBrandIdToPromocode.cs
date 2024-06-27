using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddNullableBrandIdToPromocode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promocodes_Brands_BrandId",
                table: "Promocodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "Promocodes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Promocodes_Brands_BrandId",
                table: "Promocodes",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promocodes_Brands_BrandId",
                table: "Promocodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "Promocodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Promocodes_Brands_BrandId",
                table: "Promocodes",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }
    }
}
