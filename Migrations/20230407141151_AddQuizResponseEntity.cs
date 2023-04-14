using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz_server.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizResponseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int(8)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    time = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    quizID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    quizName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()"),
                    userID = table.Column<int>(type: "int(11)", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.quizID);
                    table.ForeignKey(
                        name: "fk_user_quiz_id",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    questionID = table.Column<int>(type: "int(5)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    questionString = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    questionType = table.Column<int>(type: "int(2)", nullable: false),
                    options = table.Column<string>(type: "json", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    correctAnswer = table.Column<string>(type: "json", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    points = table.Column<float>(type: "float", nullable: false, defaultValueSql: "'1'"),
                    quizID = table.Column<int>(type: "int(11)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.questionID);
                    table.ForeignKey(
                        name: "fk_quiz_question_id",
                        column: x => x.quizID,
                        principalTable: "Quizzes",
                        principalColumn: "quizID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    scoreID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(type: "int(11)", nullable: false),
                    quizID = table.Column<int>(type: "int(11)", nullable: false),
                    Score = table.Column<float>(type: "float", nullable: false),
                    attempts = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.scoreID);
                    table.ForeignKey(
                        name: "fk_quiz_score_id",
                        column: x => x.quizID,
                        principalTable: "Quizzes",
                        principalColumn: "quizID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_score_id",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "QuizResponse",
                columns: table => new
                {
                    resID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    quizID = table.Column<int>(type: "int(11)", nullable: false),
                    questionID = table.Column<int>(type: "int(11)", nullable: false),
                    userId = table.Column<int>(type: "int(11)", nullable: false),
                    userResponse = table.Column<string>(type: "json", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResponseTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.resID);
                    table.ForeignKey(
                        name: "fk_question_response_id",
                        column: x => x.questionID,
                        principalTable: "Questions",
                        principalColumn: "questionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_quiz_response_id",
                        column: x => x.quizID,
                        principalTable: "Quizzes",
                        principalColumn: "quizID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_response_id",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "quizID",
                table: "Questions",
                column: "quizID");

            migrationBuilder.CreateIndex(
                name: "questionId",
                table: "QuizResponse",
                column: "questionID");

            migrationBuilder.CreateIndex(
                name: "quizId",
                table: "QuizResponse",
                column: "quizID");

            migrationBuilder.CreateIndex(
                name: "userId",
                table: "QuizResponse",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "userID",
                table: "Quizzes",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "quizId1",
                table: "Scores",
                column: "quizID");

            migrationBuilder.CreateIndex(
                name: "userId1",
                table: "Scores",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizResponse");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
