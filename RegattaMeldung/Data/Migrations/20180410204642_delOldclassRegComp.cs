using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class delOldclassRegComp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
