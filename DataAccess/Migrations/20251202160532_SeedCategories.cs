using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "OrderIndex", "PostCount" },
                values: new object[,]
                {
                    { new Guid("cb257c7d-aeb3-4233-a3ce-0fe419f956e0"), new DateTime(2025, 12, 2, 16, 5, 31, 657, DateTimeKind.Utc).AddTicks(8110), "Обсуждения web API и разработки", "API Docs", 1, 45 },
                    { new Guid("de026b5f-230b-4ef4-b318-a2bef0bcbdc2"), new DateTime(2025, 12, 2, 16, 5, 31, 657, DateTimeKind.Utc).AddTicks(8129), "Предложения и идеи", "Идеи", 4, 12 },
                    { new Guid("f9768b23-71a9-493f-a04e-5e5f7db54fb3"), new DateTime(2025, 12, 2, 16, 5, 31, 657, DateTimeKind.Utc).AddTicks(8124), "Задавайте вопросы", "Вопросы", 3, 67 },
                    { new Guid("fa709d13-5856-47c3-a557-3f2949fe7608"), new DateTime(2025, 12, 2, 16, 5, 31, 657, DateTimeKind.Utc).AddTicks(8119), "Общие обсуждения", "Обсуждения", 2, 23 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cb257c7d-aeb3-4233-a3ce-0fe419f956e0"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("de026b5f-230b-4ef4-b318-a2bef0bcbdc2"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f9768b23-71a9-493f-a04e-5e5f7db54fb3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fa709d13-5856-47c3-a557-3f2949fe7608"));
        }
    }
}
