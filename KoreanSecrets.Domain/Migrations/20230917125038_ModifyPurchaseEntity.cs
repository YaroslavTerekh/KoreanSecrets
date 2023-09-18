using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class ModifyPurchaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Promocode",
                table: "Purchases");

            migrationBuilder.AddColumn<Guid>(
                name: "PromocodeId",
                table: "Purchases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PromocodeId",
                table: "Purchases",
                column: "PromocodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Promocodes_PromocodeId",
                table: "Purchases",
                column: "PromocodeId",
                principalTable: "Promocodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Promocodes_PromocodeId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PromocodeId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PromocodeId",
                table: "Purchases");

            migrationBuilder.AddColumn<string>(
                name: "Promocode",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
