using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaMeldung.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Boatclasses",
                columns: table => new
                {
                    BoatclassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Seats = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boatclasses", x => x.BoatclassId);
                });

            migrationBuilder.CreateTable(
                name: "CampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<float>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampingFees", x => x.CampingFeeId);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    VNr = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Oldclasses",
                columns: table => new
                {
                    OldclassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromAge = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ToAge = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oldclasses", x => x.OldclassId);
                });

            migrationBuilder.CreateTable(
                name: "Raceclasses",
                columns: table => new
                {
                    RaceclassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Length = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raceclasses", x => x.RaceclassId);
                });

            migrationBuilder.CreateTable(
                name: "Waters",
                columns: table => new
                {
                    WaterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waters", x => x.WaterId);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Birthyear = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingFees",
                columns: table => new
                {
                    StartingFeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<float>(nullable: false),
                    BoatclassId = table.Column<int>(nullable: false),
                    OldclassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingFees", x => x.StartingFeeId);
                    table.ForeignKey(
                        name: "FK_StartingFees_Boatclasses_BoatclassId",
                        column: x => x.BoatclassId,
                        principalTable: "Boatclasses",
                        principalColumn: "BoatclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartingFees_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    CompetitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BoatclassId = table.Column<int>(nullable: false),
                    RaceclassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.CompetitionId);
                    table.ForeignKey(
                        name: "FK_Competitions_Boatclasses_BoatclassId",
                        column: x => x.BoatclassId,
                        principalTable: "Boatclasses",
                        principalColumn: "BoatclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Competitions_Raceclasses_RaceclassId",
                        column: x => x.RaceclassId,
                        principalTable: "Raceclasses",
                        principalColumn: "RaceclassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regattas",
                columns: table => new
                {
                    RegattaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Accomodation = table.Column<string>(nullable: true),
                    Awards = table.Column<string>(nullable: true),
                    Catering = table.Column<string>(nullable: true),
                    ClubId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    Judge = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ReportAddress = table.Column<string>(nullable: true),
                    ReportFax = table.Column<string>(nullable: true),
                    ReportMail = table.Column<string>(nullable: true),
                    ReportOpening = table.Column<DateTime>(nullable: false),
                    ReportSchedule = table.Column<DateTime>(nullable: false),
                    ReportTel = table.Column<string>(nullable: true),
                    ReportText = table.Column<string>(nullable: true),
                    ScheduleText = table.Column<string>(nullable: true),
                    Security = table.Column<string>(nullable: true),
                    Startslots = table.Column<int>(nullable: false),
                    SubscriberFee = table.Column<float>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    WaterId = table.Column<int>(nullable: false),
                    Waterdepth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regattas", x => x.RegattaId);
                    table.ForeignKey(
                        name: "FK_Regattas_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Regattas_Waters_WaterId",
                        column: x => x.WaterId,
                        principalTable: "Waters",
                        principalColumn: "WaterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegattaCampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false),
                    CampingFeeId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaCampingFees", x => new { x.CampingFeeId, x.RegattaId });
                    table.UniqueConstraint("AK_RegattaCampingFees_CampingFeeId", x => x.CampingFeeId);
                    table.ForeignKey(
                        name: "FK_RegattaCampingFees_CampingFees_CampingFeeId1",
                        column: x => x.CampingFeeId1,
                        principalTable: "CampingFees",
                        principalColumn: "CampingFeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaCampingFees_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaClubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaClubs", x => new { x.ClubId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaClubs_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaClubs_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaCompetitions",
                columns: table => new
                {
                    CompetitionId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaCompetitions", x => new { x.CompetitionId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaCompetitions_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaCompetitions_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaOldclasses",
                columns: table => new
                {
                    OldclassId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaOldclasses", x => new { x.OldclassId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaOldclasses_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaOldclasses_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaStartingFees",
                columns: table => new
                {
                    StartingFeeId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaStartingFees", x => new { x.StartingFeeId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaStartingFees_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaStartingFees_StartingFees_StartingFeeId",
                        column: x => x.StartingFeeId,
                        principalTable: "StartingFees",
                        principalColumn: "StartingFeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboats",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(nullable: false),
                    CompetitionId = table.Column<int>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboats", x => x.ReportedStartboatId);
                    table.ForeignKey(
                        name: "FK_ReportedStartboats_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboats_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboats_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboatMembers",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    ReportedStartboatMemberId = table.Column<int>(nullable: false),
                    Seatnumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboatMembers", x => new { x.ReportedStartboatId, x.MemberId });
                    table.UniqueConstraint("AK_ReportedStartboatMembers_ReportedStartboatMemberId", x => x.ReportedStartboatMemberId);
                    table.ForeignKey(
                        name: "FK_ReportedStartboatMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboatMembers_ReportedStartboats_ReportedStartboatId",
                        column: x => x.ReportedStartboatId,
                        principalTable: "ReportedStartboats",
                        principalColumn: "ReportedStartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_BoatclassId",
                table: "Competitions",
                column: "BoatclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_RaceclassId",
                table: "Competitions",
                column: "RaceclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_ClubId",
                table: "Members",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCampingFees_CampingFeeId1",
                table: "RegattaCampingFees",
                column: "CampingFeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCampingFees_RegattaId",
                table: "RegattaCampingFees",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaClubs_RegattaId",
                table: "RegattaClubs",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCompetitions_RegattaId",
                table: "RegattaCompetitions",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaOldclasses_RegattaId",
                table: "RegattaOldclasses",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_Regattas_ClubId",
                table: "Regattas",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Regattas_WaterId",
                table: "Regattas",
                column: "WaterId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaStartingFees_RegattaId",
                table: "RegattaStartingFees",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboatMembers_MemberId",
                table: "ReportedStartboatMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_ClubId",
                table: "ReportedStartboats",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_CompetitionId",
                table: "ReportedStartboats",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_RegattaId",
                table: "ReportedStartboats",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_BoatclassId",
                table: "StartingFees",
                column: "BoatclassId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_OldclassId",
                table: "StartingFees",
                column: "OldclassId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RegattaCampingFees");

            migrationBuilder.DropTable(
                name: "RegattaClubs");

            migrationBuilder.DropTable(
                name: "RegattaCompetitions");

            migrationBuilder.DropTable(
                name: "RegattaOldclasses");

            migrationBuilder.DropTable(
                name: "RegattaStartingFees");

            migrationBuilder.DropTable(
                name: "ReportedStartboatMembers");

            migrationBuilder.DropTable(
                name: "CampingFees");

            migrationBuilder.DropTable(
                name: "StartingFees");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "ReportedStartboats");

            migrationBuilder.DropTable(
                name: "Oldclasses");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Regattas");

            migrationBuilder.DropTable(
                name: "Boatclasses");

            migrationBuilder.DropTable(
                name: "Raceclasses");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Waters");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
