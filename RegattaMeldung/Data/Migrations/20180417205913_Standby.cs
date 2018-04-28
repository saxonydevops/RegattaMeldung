using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class Standby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportedStartboatStandbys",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    Standbynumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboatStandbys", x => new { x.ReportedStartboatId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_ReportedStartboatStandbys_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboatStandbys_ReportedStartboats_ReportedStartboatId",
                        column: x => x.ReportedStartboatId,
                        principalTable: "ReportedStartboats",
                        principalColumn: "ReportedStartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboatStandbys_MemberId",
                table: "ReportedStartboatStandbys",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportedStartboatStandbys");
        }
    }
}
