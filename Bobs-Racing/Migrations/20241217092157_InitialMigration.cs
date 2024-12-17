using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bobs_Racing.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWin",
                table: "Bets",
                type: "bit",
                nullable: false,
                defaultValue: false); // Default value is 0 (false)
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWin",
                table: "Bets");
        }
    }
}
