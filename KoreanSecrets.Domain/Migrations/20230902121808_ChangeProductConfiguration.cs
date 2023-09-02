using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class ChangeProductConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Countries_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Demands_BrandId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CountryId",
                table: "Products",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DemandId",
                table: "Products",
                column: "DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Countries_CountryId",
                table: "Products",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Demands_DemandId",
                table: "Products",
                column: "DemandId",
                principalTable: "Demands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Countries_CountryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Demands_DemandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CountryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DemandId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Countries_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Demands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Demands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
