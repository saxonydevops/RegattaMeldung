using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class deletedstartboats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedStartboats",
                columns: table => new
                {
                    DeletedStartboatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    RegattaId = table.Column<int>(nullable: false),
                    ReportedRaceId = table.Column<int>(nullable: false),
                    ReportedStartboatId = table.Column<int>(nullable: false),
                    deleteDate = table.Column<DateTime>(nullable: false),
                    wasLate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedStartboats", x => x.DeletedStartboatId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedStartboats");
        }
    }
}
