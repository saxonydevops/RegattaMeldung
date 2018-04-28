using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class CompetitionOhneOldclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Oldclasses_OldclassId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_OldclassId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "OldclassId",
                table: "Competitions");

            migrationBuilder.AddColumn<int>(
                name: "OldclassId",
                table: "RegattaCompetitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCompetitions_OldclassId",
                table: "RegattaCompetitions",
                column: "OldclassId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegattaCompetitions_Oldclasses_OldclassId",
                table: "RegattaCompetitions",
                column: "OldclassId",
                principalTable: "Oldclasses",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegattaCompetitions_Oldclasses_OldclassId",
                table: "RegattaCompetitions");

            migrationBuilder.DropIndex(
                name: "IX_RegattaCompetitions_OldclassId",
                table: "RegattaCompetitions");

            migrationBuilder.DropColumn(
                name: "OldclassId",
                table: "RegattaCompetitions");

            migrationBuilder.AddColumn<int>(
                name: "OldclassId",
                table: "Competitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_OldclassId",
                table: "Competitions",
                column: "OldclassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Oldclasses_OldclassId",
                table: "Competitions",
                column: "OldclassId",
                principalTable: "Oldclasses",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
