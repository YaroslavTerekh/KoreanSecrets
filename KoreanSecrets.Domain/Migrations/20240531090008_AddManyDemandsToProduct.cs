using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddManyDemandsToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Demands_DemandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DemandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DemandId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductDemand",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DemandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDemand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDemand_Demands_DemandId",
                        column: x => x.DemandId,
                        principalTable: "Demands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDemand_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDemand_DemandId",
                table: "ProductDemand",
                column: "DemandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDemand_ProductId",
                table: "ProductDemand",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDemand");

            migrationBuilder.AddColumn<Guid>(
                name: "DemandId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DemandId",
                table: "Products",
                column: "DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Demands_DemandId",
                table: "Products",
                column: "DemandId",
                principalTable: "Demands",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
