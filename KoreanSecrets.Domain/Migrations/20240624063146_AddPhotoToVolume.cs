using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddPhotoToVolume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VolumePhotoId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_VolumePhotoId",
                table: "Files",
                column: "VolumePhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Volume_VolumePhotoId",
                table: "Files",
                column: "VolumePhotoId",
                principalTable: "Volume",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Volume_VolumePhotoId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_VolumePhotoId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "VolumePhotoId",
                table: "Files");
        }
    }
}
