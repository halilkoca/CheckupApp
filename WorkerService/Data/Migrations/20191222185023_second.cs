using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppUrl",
                table: "CheckApp",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppName",
                table: "CheckApp",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AppHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckAppId = table.Column<int>(nullable: false),
                    RequestDateUtc = table.Column<DateTime>(nullable: false),
                    StatusCode = table.Column<int>(nullable: false),
                    RequestId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppHistory_CheckApp_CheckAppId",
                        column: x => x.CheckAppId,
                        principalTable: "CheckApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppHistory_CheckAppId",
                table: "AppHistory",
                column: "CheckAppId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppHistory");

            migrationBuilder.AlterColumn<string>(
                name: "AppUrl",
                table: "CheckApp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "AppName",
                table: "CheckApp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512);
        }
    }
}
