using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class RegattaIdReportedRace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegattaId",
                table: "ReportedRaces",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRaces_RegattaId",
                table: "ReportedRaces",
                column: "RegattaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRaces_Regattas_RegattaId",
                table: "ReportedRaces",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRaces_Regattas_RegattaId",
                table: "ReportedRaces");

            migrationBuilder.DropIndex(
                name: "IX_ReportedRaces_RegattaId",
                table: "ReportedRaces");

            migrationBuilder.DropColumn(
                name: "RegattaId",
                table: "ReportedRaces");
        }
    }
}
