using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddBrandToPromocode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Promocodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Promocodes_BrandId",
                table: "Promocodes",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promocodes_Brands_BrandId",
                table: "Promocodes",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promocodes_Brands_BrandId",
                table: "Promocodes");

            migrationBuilder.DropIndex(
                name: "IX_Promocodes_BrandId",
                table: "Promocodes");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Promocodes");
        }
    }
}
