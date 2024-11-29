using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bobs_Racing.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceAnimal");

            migrationBuilder.DropColumn(
                name: "Checkpoints",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "Races",
                newName: "Rankings");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Animals",
                newName: "AnimalId");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Races",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "RaceAnimals",
                columns: table => new
                {
                    AnimalsAnimalId = table.Column<int>(type: "int", nullable: false),
                    RacesRaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceAnimals", x => new { x.AnimalsAnimalId, x.RacesRaceId });
                    table.ForeignKey(
                        name: "FK_RaceAnimals_Animals_AnimalsAnimalId",
                        column: x => x.AnimalsAnimalId,
                        principalTable: "Animals",
                        principalColumn: "AnimalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceAnimals_Races_RacesRaceId",
                        column: x => x.RacesRaceId,
                        principalTable: "Races",
                        principalColumn: "RaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceAnimals_RacesRaceId",
                table: "RaceAnimals",
                column: "RacesRaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceAnimals");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "Rankings",
                table: "Races",
                newName: "Result");

            migrationBuilder.RenameColumn(
                name: "AnimalId",
                table: "Animals",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Checkpoints",
                table: "Races",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RaceAnimal",
                columns: table => new
                {
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    RaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceAnimal", x => new { x.AnimalId, x.RaceId });
                    table.ForeignKey(
                        name: "FK_RaceAnimal_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceAnimal_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "RaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceAnimal_RaceId",
                table: "RaceAnimal",
                column: "RaceId");
        }
    }
}
