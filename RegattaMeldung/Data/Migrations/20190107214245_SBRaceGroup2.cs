using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaMeldung.Data.Migrations
{
    public partial class SBRaceGroup2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRaceGroup",
                table: "ReportedStartboats",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRaceGroup",
                table: "ReportedStartboats");
        }
    }
}
