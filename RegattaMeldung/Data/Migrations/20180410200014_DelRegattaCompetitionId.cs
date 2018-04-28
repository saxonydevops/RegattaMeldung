using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class DelRegattaCompetitionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegattaCompetitionId",
                table: "RegattaCompetitions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegattaCompetitionId",
                table: "RegattaCompetitions",
                nullable: false,
                defaultValue: 0);
        }
    }
}
