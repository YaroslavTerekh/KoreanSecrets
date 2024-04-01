using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddVolumeToPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VolumeId",
                table: "PurchasedProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProducts_VolumeId",
                table: "PurchasedProducts",
                column: "VolumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProducts_Volume_VolumeId",
                table: "PurchasedProducts",
                column: "VolumeId",
                principalTable: "Volume",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProducts_Volume_VolumeId",
                table: "PurchasedProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProducts_VolumeId",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "VolumeId",
                table: "PurchasedProducts");
        }
    }
}
