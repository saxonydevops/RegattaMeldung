using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaMeldung.Data.Migrations
{
    public partial class StartingFeeOldclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegattaCampingFees_CampingFees_CampingFeeId1",
                table: "RegattaCampingFees");

            migrationBuilder.DropForeignKey(
                name: "FK_StartingFees_Oldclasses_OldclassId",
                table: "StartingFees");

            migrationBuilder.DropIndex(
                name: "IX_StartingFees_OldclassId",
                table: "StartingFees");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RegattaCampingFees_CampingFeeId",
                table: "RegattaCampingFees");

            migrationBuilder.DropIndex(
                name: "IX_RegattaCampingFees_CampingFeeId1",
                table: "RegattaCampingFees");

            migrationBuilder.DropColumn(
                name: "CampingFeeId1",
                table: "RegattaCampingFees");

            migrationBuilder.RenameColumn(
                name: "OldclassId",
                table: "StartingFees",
                newName: "ToOldclassId");

            migrationBuilder.AddColumn<int>(
                name: "FromOldclassId",
                table: "StartingFees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RegattaCampingFees_CampingFees_CampingFeeId",
                table: "RegattaCampingFees",
                column: "CampingFeeId",
                principalTable: "CampingFees",
                principalColumn: "CampingFeeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegattaCampingFees_CampingFees_CampingFeeId",
                table: "RegattaCampingFees");

            migrationBuilder.DropColumn(
                name: "FromOldclassId",
                table: "StartingFees");

            migrationBuilder.RenameColumn(
                name: "ToOldclassId",
                table: "StartingFees",
                newName: "OldclassId");

            migrationBuilder.AddColumn<int>(
                name: "CampingFeeId1",
                table: "RegattaCampingFees",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RegattaCampingFees_CampingFeeId",
                table: "RegattaCampingFees",
                column: "CampingFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_OldclassId",
                table: "StartingFees",
                column: "OldclassId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCampingFees_CampingFeeId1",
                table: "RegattaCampingFees",
                column: "CampingFeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RegattaCampingFees_CampingFees_CampingFeeId1",
                table: "RegattaCampingFees",
                column: "CampingFeeId1",
                principalTable: "CampingFees",
                principalColumn: "CampingFeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StartingFees_Oldclasses_OldclassId",
                table: "StartingFees",
                column: "OldclassId",
                principalTable: "Oldclasses",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
