using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaMeldung.Data.Migrations
{
    public partial class RegattaOrgCat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Regattas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organizer",
                table: "Regattas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartersLastYear",
                table: "Regattas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "StartersLastYear",
                table: "Regattas");
        }
    }
}
