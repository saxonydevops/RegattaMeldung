using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaMeldung.Data.Migrations
{
    public partial class member58 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clubs",
                keyColumn: "ClubId",
                keyValue: 1,
                columns: new[] { "City", "Name", "ShortName", "VNr" },
                values: new object[] { "Platzhalter", "Platzhalter", "Platzhalter", "00-000" });

            migrationBuilder.UpdateData(
                table: "Clubs",
                keyColumn: "ClubId",
                keyValue: 294,
                columns: new[] { "City", "Name", "ShortName", "VNr" },
                values: new object[] { "Ansbach", "Kanu-Sportclub Ansbach", "Ansbach", "02-001" });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "Birthyear", "ClubId", "FirstName", "Gender", "LastName", "RentedToClubId", "isRented" },
                values: new object[,]
                {
                    { 2, 2000, 294, "WIRD", "M", "GESUCHT", 0, false },
                    { 3, 2000, 294, "WIRD", "M", "GESUCHT", 0, false },
                    { 4, 2000, 294, "WIRD", "M", "GESUCHT", 0, false },
                    { 5, 2000, 294, "WIRD", "M", "GESUCHT", 0, false },
                    { 6, 2000, 294, "WIRD", "M", "GESUCHT", 0, false },
                    { 7, 2000, 294, "WIRD", "M", "GESUCHT", 0, false },
                    { 8, 2000, 294, "WIRD", "M", "GESUCHT", 0, false }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Clubs",
                keyColumn: "ClubId",
                keyValue: 1,
                columns: new[] { "City", "Name", "ShortName", "VNr" },
                values: new object[] { "Ansbach", "Kanu-Sportclub Ansbach", "Ansbach", "02-001" });

            migrationBuilder.UpdateData(
                table: "Clubs",
                keyColumn: "ClubId",
                keyValue: 294,
                columns: new[] { "City", "Name", "ShortName", "VNr" },
                values: new object[] { "Platzhalter", "Platzhalter", "Platzhalter", "00-000" });
        }
    }
}
