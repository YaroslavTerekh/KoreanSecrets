using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class ModifyBucketEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BucketProduct");

            migrationBuilder.AddColumn<Guid>(
                name: "BucketId",
                table: "PurchasedProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Buckets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProducts_BucketId",
                table: "PurchasedProducts",
                column: "BucketId");

            migrationBuilder.CreateIndex(
                name: "IX_Buckets_ProductId",
                table: "Buckets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProducts_Buckets_BucketId",
                table: "PurchasedProducts",
                column: "BucketId",
                principalTable: "Buckets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProducts_Buckets_BucketId",
                table: "PurchasedProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProducts_BucketId",
                table: "PurchasedProducts");

            migrationBuilder.DropIndex(
                name: "IX_Buckets_ProductId",
                table: "Buckets");

            migrationBuilder.DropColumn(
                name: "BucketId",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Buckets");

            migrationBuilder.CreateTable(
                name: "BucketProduct",
                columns: table => new
                {
                    BucketsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BucketProduct", x => new { x.BucketsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_BucketProduct_Buckets_BucketsId",
                        column: x => x.BucketsId,
                        principalTable: "Buckets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BucketProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BucketProduct_ProductsId",
                table: "BucketProduct",
                column: "ProductsId");
        }
    }
}
