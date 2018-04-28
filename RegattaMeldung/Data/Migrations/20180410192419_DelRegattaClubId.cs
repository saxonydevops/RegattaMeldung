using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class DelRegattaClubId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RegattaClubs_RegattaClubId",
                table: "RegattaClubs");

            migrationBuilder.DropColumn(
                name: "RegattaClubId",
                table: "RegattaClubs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegattaClubId",
                table: "RegattaClubs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RegattaClubs_RegattaClubId",
                table: "RegattaClubs",
                column: "RegattaClubId");
        }
    }
}
