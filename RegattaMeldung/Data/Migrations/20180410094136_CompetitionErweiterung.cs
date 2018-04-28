using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class CompetitionErweiterung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_Competitions_CompetitionId",
                table: "ReportedStartboats");

            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "ReportedStartboats",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "RegattaCompetitionCompetitionId",
                table: "ReportedStartboats",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegattaCompetitionId",
                table: "ReportedStartboats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegattaCompetitionRegattaId",
                table: "ReportedStartboats",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegattaCompetitionId",
                table: "RegattaCompetitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_RegattaCompetitionCompetitionId_RegattaCompetitionRegattaId",
                table: "ReportedStartboats",
                columns: new[] { "RegattaCompetitionCompetitionId", "RegattaCompetitionRegattaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_Competitions_CompetitionId",
                table: "ReportedStartboats",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_RegattaCompetitions_RegattaCompetitionCompetitionId_RegattaCompetitionRegattaId",
                table: "ReportedStartboats",
                columns: new[] { "RegattaCompetitionCompetitionId", "RegattaCompetitionRegattaId" },
                principalTable: "RegattaCompetitions",
                principalColumns: new[] { "CompetitionId", "RegattaId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_Competitions_CompetitionId",
                table: "ReportedStartboats");

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
                name: "RegattaCompetitionId",
                table: "ReportedStartboats");

            migrationBuilder.DropColumn(
                name: "RegattaCompetitionRegattaId",
                table: "ReportedStartboats");

            migrationBuilder.DropColumn(
                name: "RegattaCompetitionId",
                table: "RegattaCompetitions");

            migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "ReportedStartboats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_Competitions_CompetitionId",
                table: "ReportedStartboats",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
