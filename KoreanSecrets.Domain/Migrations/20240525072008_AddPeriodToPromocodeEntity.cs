using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoreanSecrets.Domain.Migrations
{
    public partial class AddPeriodToPromocodeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Promocodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Promocodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Promocodes");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Promocodes");
        }
    }
}
