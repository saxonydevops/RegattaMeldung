using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaMeldung.Data.Migrations
{
    public partial class gesucht : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 99999);

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "Birthyear", "ClubId", "FirstName", "Gender", "LastName", "RentedToClubId", "isRented" },
                values: new object[] { 1, 2000, 294, "WIRD", "M", "GESUCHT", 0, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "Birthyear", "ClubId", "FirstName", "Gender", "LastName", "RentedToClubId", "isRented" },
                values: new object[] { 99999, 2000, 294, "WIRD", "M", "GESUCHT", 0, false });
        }
    }
}
