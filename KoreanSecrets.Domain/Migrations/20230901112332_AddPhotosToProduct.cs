using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddPhotosToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductPhotoId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Files_ProductPhotoId",
                table: "Files",
                column: "ProductPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Products_ProductPhotoId",
                table: "Files",
                column: "ProductPhotoId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Products_ProductPhotoId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ProductPhotoId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ProductPhotoId",
                table: "Files");
        }
    }
}
