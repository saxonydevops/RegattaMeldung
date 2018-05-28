using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class freestartslots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NoStartslot",
                table: "ReportedStartboats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FreeStartslots",
                table: "ReportedRaces",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoStartslot",
                table: "ReportedStartboats");

            migrationBuilder.DropColumn(
                name: "FreeStartslots",
                table: "ReportedRaces");
        }
    }
}
