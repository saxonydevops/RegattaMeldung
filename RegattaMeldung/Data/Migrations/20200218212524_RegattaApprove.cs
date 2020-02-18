using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaMeldung.Data.Migrations
{
    public partial class RegattaApprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentYear",
                table: "Members");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Regattas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Regattas");

            migrationBuilder.AddColumn<int>(
                name: "RentYear",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
