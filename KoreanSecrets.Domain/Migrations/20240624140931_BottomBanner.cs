using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class BottomBanner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BottomBanners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottomBanners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BottomBannerPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BottomBannerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSmall = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottomBannerPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BottomBannerPhotos_BottomBanners_BottomBannerId",
                        column: x => x.BottomBannerId,
                        principalTable: "BottomBanners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BottomBannerPhotos_Files_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BottomBannerPhotos_BottomBannerId",
                table: "BottomBannerPhotos",
                column: "BottomBannerId");

            migrationBuilder.CreateIndex(
                name: "IX_BottomBannerPhotos_PhotoId",
                table: "BottomBannerPhotos",
                column: "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BottomBannerPhotos");

            migrationBuilder.DropTable(
                name: "BottomBanners");
        }
    }
}
