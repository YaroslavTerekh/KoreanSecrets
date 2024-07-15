using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddAdminBucketEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "BucketId",
                table: "BucketProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminBucketId",
                table: "BucketProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminBucketId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdminBuckets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminBuckets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminBuckets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BucketProducts_AdminBucketId",
                table: "BucketProducts",
                column: "AdminBucketId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminBuckets_UserId",
                table: "AdminBuckets",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BucketProducts_AdminBuckets_AdminBucketId",
                table: "BucketProducts",
                column: "AdminBucketId",
                principalTable: "AdminBuckets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BucketProducts_AdminBuckets_AdminBucketId",
                table: "BucketProducts");

            migrationBuilder.DropTable(
                name: "AdminBuckets");

            migrationBuilder.DropIndex(
                name: "IX_BucketProducts_AdminBucketId",
                table: "BucketProducts");

            migrationBuilder.DropColumn(
                name: "AdminBucketId",
                table: "BucketProducts");

            migrationBuilder.DropColumn(
                name: "AdminBucketId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BucketId",
                table: "BucketProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
