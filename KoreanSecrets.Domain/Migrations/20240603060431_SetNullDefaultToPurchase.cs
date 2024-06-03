using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class SetNullDefaultToPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Promocodes_PromocodeId",
                table: "Purchases");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Promocodes_PromocodeId",
                table: "Purchases",
                column: "PromocodeId",
                principalTable: "Promocodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Promocodes_PromocodeId",
                table: "Purchases");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Promocodes_PromocodeId",
                table: "Purchases",
                column: "PromocodeId",
                principalTable: "Promocodes",
                principalColumn: "Id");
        }
    }
}
