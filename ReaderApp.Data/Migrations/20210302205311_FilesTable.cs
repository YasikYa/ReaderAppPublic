using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReaderApp.Data.Migrations
{
    public partial class FilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4c834256-d995-46e0-8e3a-d783be6feaae"));

            migrationBuilder.CreateTable(
                name: "TextFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { new Guid("63a3567f-afdb-42af-9df6-a8b2b103edc9"), "test@mail.com", "Test User", "12345" });

            migrationBuilder.CreateIndex(
                name: "IX_TextFiles_UserId",
                table: "TextFiles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextFiles");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("63a3567f-afdb-42af-9df6-a8b2b103edc9"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { new Guid("4c834256-d995-46e0-8e3a-d783be6feaae"), "test@mail.com", "Test User", "12345" });
        }
    }
}
