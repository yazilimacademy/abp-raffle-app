using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YazilimAcademy.ABPRaffleApp.Migrations
{
    /// <inheritdoc />
    public partial class RaffleEntitiesCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRaffles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRaffles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RaffleId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppParticipants_AppRaffles_RaffleId",
                        column: x => x.RaffleId,
                        principalTable: "AppRaffles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRaffleResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RaffleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RaffleId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    IsWinner = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRaffleResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRaffleResults_AppParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "AppParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRaffleResults_AppParticipants_ParticipantId1",
                        column: x => x.ParticipantId1,
                        principalTable: "AppParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRaffleResults_AppRaffles_RaffleId",
                        column: x => x.RaffleId,
                        principalTable: "AppRaffles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRaffleResults_AppRaffles_RaffleId1",
                        column: x => x.RaffleId1,
                        principalTable: "AppRaffles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppParticipants_Email",
                table: "AppParticipants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppParticipants_FullName",
                table: "AppParticipants",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_AppParticipants_RaffleId",
                table: "AppParticipants",
                column: "RaffleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRaffleResults_ParticipantId",
                table: "AppRaffleResults",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRaffleResults_ParticipantId1",
                table: "AppRaffleResults",
                column: "ParticipantId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppRaffleResults_RaffleId",
                table: "AppRaffleResults",
                column: "RaffleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRaffleResults_RaffleId1",
                table: "AppRaffleResults",
                column: "RaffleId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRaffleResults");

            migrationBuilder.DropTable(
                name: "AppParticipants");

            migrationBuilder.DropTable(
                name: "AppRaffles");
        }
    }
}
