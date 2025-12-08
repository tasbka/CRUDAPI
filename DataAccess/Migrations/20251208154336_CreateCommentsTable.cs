using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateCommentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedAt",
                table: "Comments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NoteId",
                table: "Comments",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5a05b203-07f7-4ed3-b876-dd36b6aee44a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("887472e7-a834-4bf4-a460-fc2166178187"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ba1cc809-c146-4690-9311-1034cb8e041c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e650cbba-a0ba-4dee-8759-02680826dd68"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "OrderIndex", "PostCount" },
                values: new object[,]
                {
                    { new Guid("11587dd6-3b06-4ecc-86ea-1eec2e919436"), new DateTime(2025, 12, 7, 19, 20, 6, 262, DateTimeKind.Utc).AddTicks(5668), "Предложения и идеи", "Идеи", 4, 0 },
                    { new Guid("4b6e46ca-da99-4371-bd1a-c1f40e5a9a11"), new DateTime(2025, 12, 7, 19, 20, 6, 262, DateTimeKind.Utc).AddTicks(5654), "Задавайте вопросы", "Вопросы", 3, 0 },
                    { new Guid("4e7a52bb-4c31-4b01-8ee2-4c2145f8836d"), new DateTime(2025, 12, 7, 19, 20, 6, 262, DateTimeKind.Utc).AddTicks(5652), "Общие обсуждения", "Обсуждения", 2, 0 },
                    { new Guid("d9a0a2cf-af10-47ba-a2d2-d2365c6d20cc"), new DateTime(2025, 12, 7, 19, 20, 6, 262, DateTimeKind.Utc).AddTicks(5647), "Обсуждения web API и разработки", "API Docs", 1, 0 }
                });
        }
    }
}
