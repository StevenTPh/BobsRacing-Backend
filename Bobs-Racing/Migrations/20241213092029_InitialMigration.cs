using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bobs_Racing.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Change Credits column in Users table to FLOAT
            migrationBuilder.AlterColumn<double>(
                name: "Credits",
                table: "Users",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove IsActive column from Bets table
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Bets");

            // Remove IsFinished column from Races table
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Races");

            // Remove FinishTime column from RaceAthletes table
            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "RaceAthletes");

            // Revert Credits column in Users table back to INT
            migrationBuilder.AlterColumn<int>(
                name: "Credits",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

    }
}
