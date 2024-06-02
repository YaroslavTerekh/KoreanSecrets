using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddBucketProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProducts_Buckets_BucketId",
                table: "PurchasedProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProducts_BucketId",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "BucketId",
                table: "PurchasedProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                table: "PurchasedProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BucketProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BucketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VolumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BucketProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BucketProducts_Buckets_BucketId",
                        column: x => x.BucketId,
                        principalTable: "Buckets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BucketProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BucketProducts_Volume_VolumeId",
                        column: x => x.VolumeId,
                        principalTable: "Volume",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BucketProducts_BucketId",
                table: "BucketProducts",
                column: "BucketId");

            migrationBuilder.CreateIndex(
                name: "IX_BucketProducts_ProductId",
                table: "BucketProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BucketProducts_VolumeId",
                table: "BucketProducts",
                column: "VolumeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BucketProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                table: "PurchasedProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BucketId",
                table: "PurchasedProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProducts_BucketId",
                table: "PurchasedProducts",
                column: "BucketId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProducts_Buckets_BucketId",
                table: "PurchasedProducts",
                column: "BucketId",
                principalTable: "Buckets",
                principalColumn: "Id");
        }
    }
}
