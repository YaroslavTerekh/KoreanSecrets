using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddPhotoToBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandPhotoId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Files_BrandPhotoId",
                table: "Files",
                column: "BrandPhotoId",
                unique: true,
                filter: "[BrandPhotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Brands_BrandPhotoId",
                table: "Files",
                column: "BrandPhotoId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Brands_BrandPhotoId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_BrandPhotoId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "BrandPhotoId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Brands");
        }
    }
}
