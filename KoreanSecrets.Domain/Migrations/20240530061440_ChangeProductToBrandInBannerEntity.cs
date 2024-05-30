using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class ChangeProductToBrandInBannerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Products_ProductId",
                table: "Banners");

            migrationBuilder.DropIndex(
                name: "IX_Banners_ProductId",
                table: "Banners");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Banners",
                newName: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_BrandId",
                table: "Banners",
                column: "BrandId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Brands_BrandId",
                table: "Banners",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Brands_BrandId",
                table: "Banners");

            migrationBuilder.DropIndex(
                name: "IX_Banners_BrandId",
                table: "Banners");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "Banners",
                newName: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_ProductId",
                table: "Banners",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Products_ProductId",
                table: "Banners",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
