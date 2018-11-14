using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaMeldung.Data.Migrations
{
    public partial class InviteHome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Invited",
                table: "RegattaClubs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EMail",
                table: "Clubs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invited",
                table: "RegattaClubs");

            migrationBuilder.DropColumn(
                name: "EMail",
                table: "Clubs");
        }
    }
}
