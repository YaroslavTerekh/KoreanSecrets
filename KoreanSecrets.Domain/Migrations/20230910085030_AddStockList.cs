using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddStockList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductUserWaitForStock",
                columns: table => new
                {
                    ProductsWaitingForStockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWaitingForStockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUserWaitForStock", x => new { x.ProductsWaitingForStockId, x.UsersWaitingForStockId });
                    table.ForeignKey(
                        name: "FK_ProductUserWaitForStock_AspNetUsers_UsersWaitingForStockId",
                        column: x => x.UsersWaitingForStockId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUserWaitForStock_Products_ProductsWaitingForStockId",
                        column: x => x.ProductsWaitingForStockId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductUserWaitForStock_UsersWaitingForStockId",
                table: "ProductUserWaitForStock",
                column: "UsersWaitingForStockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductUserWaitForStock");
        }
    }
}
