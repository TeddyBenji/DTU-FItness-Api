using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DTUFItnessApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseMetricTableMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clubs",
                columns: table => new
                {
                    ClubID = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClubName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OwnerUserId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clubs", x => x.ClubID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "exercises",
                columns: table => new
                {
                    ExerciseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercises", x => x.ExerciseID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "metrics",
                columns: table => new
                {
                    MetricID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metrics", x => x.MetricID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    IdentityUserID = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileID = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => x.IdentityUserID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clubmembers",
                columns: table => new
                {
                    ClubMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MemberId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clubmembers", x => x.ClubMemberId);
                    table.ForeignKey(
                        name: "FK_clubmembers_clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "clubs",
                        principalColumn: "ClubID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_clubmembers_user_profiles_MemberId",
                        column: x => x.MemberId,
                        principalTable: "user_profiles",
                        principalColumn: "IdentityUserID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "exercise_logs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExerciseID = table.Column<int>(type: "int", nullable: false),
                    ExerciseDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_logs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_exercise_logs_exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "exercises",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exercise_logs_user_profiles_UserID",
                        column: x => x.UserID,
                        principalTable: "user_profiles",
                        principalColumn: "IdentityUserID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "exercise_metrics",
                columns: table => new
                {
                    ExerciseMetricID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExerciseLogID = table.Column<int>(type: "int", nullable: false),
                    MetricID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_metrics", x => x.ExerciseMetricID);
                    table.ForeignKey(
                        name: "FK_exercise_metrics_exercise_logs_ExerciseLogID",
                        column: x => x.ExerciseLogID,
                        principalTable: "exercise_logs",
                        principalColumn: "LogID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exercise_metrics_metrics_MetricID",
                        column: x => x.MetricID,
                        principalTable: "metrics",
                        principalColumn: "MetricID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_clubmembers_ClubId",
                table: "clubmembers",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_clubmembers_MemberId",
                table: "clubmembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_logs_ExerciseID",
                table: "exercise_logs",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_logs_UserID",
                table: "exercise_logs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_metrics_ExerciseLogID",
                table: "exercise_metrics",
                column: "ExerciseLogID");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_metrics_MetricID",
                table: "exercise_metrics",
                column: "MetricID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clubmembers");

            migrationBuilder.DropTable(
                name: "exercise_metrics");

            migrationBuilder.DropTable(
                name: "clubs");

            migrationBuilder.DropTable(
                name: "exercise_logs");

            migrationBuilder.DropTable(
                name: "metrics");

            migrationBuilder.DropTable(
                name: "exercises");

            migrationBuilder.DropTable(
                name: "user_profiles");
        }
    }
}
