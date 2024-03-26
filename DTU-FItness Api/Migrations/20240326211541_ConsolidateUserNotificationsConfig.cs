using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DTUFItnessApi.Migrations
{
    /// <inheritdoc />
    public partial class ConsolidateUserNotificationsConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClubID = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_events_clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "clubs",
                        principalColumn: "ClubID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_notifications_events_EventID",
                        column: x => x.EventID,
                        principalTable: "events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_notifications",
                columns: table => new
                {
                    UserNotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotificationID = table.Column<int>(type: "int", nullable: false),
                    IdentityUserID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotificationID1 = table.Column<int>(type: "int", nullable: false),
                    UserIdentityUserID = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_notifications", x => x.UserNotificationID);
                    table.ForeignKey(
                        name: "FK_user_notifications_notifications_NotificationID",
                        column: x => x.NotificationID,
                        principalTable: "notifications",
                        principalColumn: "NotificationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_notifications_notifications_NotificationID1",
                        column: x => x.NotificationID1,
                        principalTable: "notifications",
                        principalColumn: "NotificationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_notifications_user_profiles_IdentityUserID",
                        column: x => x.IdentityUserID,
                        principalTable: "user_profiles",
                        principalColumn: "IdentityUserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_notifications_user_profiles_UserIdentityUserID",
                        column: x => x.UserIdentityUserID,
                        principalTable: "user_profiles",
                        principalColumn: "IdentityUserID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_events_ClubID",
                table: "events",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_EventID",
                table: "notifications",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_IdentityUserID",
                table: "user_notifications",
                column: "IdentityUserID");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_NotificationID",
                table: "user_notifications",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_NotificationID1",
                table: "user_notifications",
                column: "NotificationID1");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_UserIdentityUserID",
                table: "user_notifications",
                column: "UserIdentityUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_notifications");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
