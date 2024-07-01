using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class ChangePurchaseProductLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProducts_Products_ProductId",
                table: "PurchasedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProducts_Volume_VolumeId",
                table: "PurchasedProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProducts_ProductId",
                table: "PurchasedProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProducts_VolumeId",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "VolumeId",
                table: "PurchasedProducts");

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "PurchasedProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductIdentify",
                table: "PurchasedProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle",
                table: "PurchasedProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Volume",
                table: "PurchasedProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VolumeIdentify",
                table: "PurchasedProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "ProductIdentify",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "ProductTitle",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "VolumeIdentify",
                table: "PurchasedProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "PurchasedProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VolumeId",
                table: "PurchasedProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProducts_ProductId",
                table: "PurchasedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProducts_VolumeId",
                table: "PurchasedProducts",
                column: "VolumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProducts_Products_ProductId",
                table: "PurchasedProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProducts_Volume_VolumeId",
                table: "PurchasedProducts",
                column: "VolumeId",
                principalTable: "Volume",
                principalColumn: "Id");
        }
    }
}
