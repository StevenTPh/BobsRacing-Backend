using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bobs_Racing.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Add or change tables

            // Change Column
            migrationBuilder.AlterColumn<double>(
                name: "PotentialPayout",
                table: "Bets",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove from tables
        }

    }
}
