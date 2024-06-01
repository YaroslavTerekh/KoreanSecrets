using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddTemporaryCodeToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TemporaryCode",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemporaryCode",
                table: "AspNetUsers");
        }
    }
}
