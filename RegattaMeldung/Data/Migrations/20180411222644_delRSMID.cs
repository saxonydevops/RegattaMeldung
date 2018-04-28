using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class delRSMID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ReportedStartboatMembers_ReportedStartboatMemberId",
                table: "ReportedStartboatMembers");

            migrationBuilder.DropColumn(
                name: "ReportedStartboatMemberId",
                table: "ReportedStartboatMembers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportedStartboatMemberId",
                table: "ReportedStartboatMembers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ReportedStartboatMembers_ReportedStartboatMemberId",
                table: "ReportedStartboatMembers",
                column: "ReportedStartboatMemberId");
        }
    }
}
