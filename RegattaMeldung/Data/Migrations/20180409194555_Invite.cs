using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class Invite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "RegattaClubs",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RegattaClubs_RegattaClubId",
                table: "RegattaClubs");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RegattaClubs");

            migrationBuilder.DropColumn(
                name: "RegattaClubId",
                table: "RegattaClubs");
        }
    }
}
