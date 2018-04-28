using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class DiverseStartboot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_RegattaCompetitions_RegattaCompetitionCompetitionId_RegattaCompetitionRegattaId",
                table: "ReportedStartboats");

            migrationBuilder.DropIndex(
                name: "IX_ReportedStartboats_RegattaCompetitionCompetitionId_RegattaCompetitionRegattaId",
                table: "ReportedStartboats");

            migrationBuilder.DropColumn(
                name: "RegattaCompetitionCompetitionId",
                table: "ReportedStartboats");

            migrationBuilder.DropColumn(
                name: "RegattaCompetitionRegattaId",
                table: "ReportedStartboats");

            migrationBuilder.RenameColumn(
                name: "RegattaCompetitionId",
                table: "ReportedStartboats",
                newName: "ReportedRaceId");

            migrationBuilder.CreateTable(
                name: "ReportedRaces",
                columns: table => new
                {
                    ReportedRaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompetitionId = table.Column<int>(nullable: false),
                    OldclassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedRaces", x => x.ReportedRaceId);
                    table.ForeignKey(
                        name: "FK_ReportedRaces_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedRaces_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_ReportedRaceId",
                table: "ReportedStartboats",
                column: "ReportedRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRaces_CompetitionId",
                table: "ReportedRaces",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRaces_OldclassId",
                table: "ReportedRaces",
                column: "OldclassId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_ReportedRaces_ReportedRaceId",
                table: "ReportedStartboats",
                column: "ReportedRaceId",
                principalTable: "ReportedRaces",
                principalColumn: "ReportedRaceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_ReportedRaces_ReportedRaceId",
                table: "ReportedStartboats");

            migrationBuilder.DropTable(
                name: "ReportedRaces");

            migrationBuilder.DropIndex(
                name: "IX_ReportedStartboats_ReportedRaceId",
                table: "ReportedStartboats");

            migrationBuilder.RenameColumn(
                name: "ReportedRaceId",
                table: "ReportedStartboats",
                newName: "RegattaCompetitionId");

            migrationBuilder.AddColumn<int>(
                name: "RegattaCompetitionCompetitionId",
                table: "ReportedStartboats",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegattaCompetitionRegattaId",
                table: "ReportedStartboats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_RegattaCompetitionCompetitionId_RegattaCompetitionRegattaId",
                table: "ReportedStartboats",
                columns: new[] { "RegattaCompetitionCompetitionId", "RegattaCompetitionRegattaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_RegattaCompetitions_RegattaCompetitionCompetitionId_RegattaCompetitionRegattaId",
                table: "ReportedStartboats",
                columns: new[] { "RegattaCompetitionCompetitionId", "RegattaCompetitionRegattaId" },
                principalTable: "RegattaCompetitions",
                principalColumns: new[] { "CompetitionId", "RegattaId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
