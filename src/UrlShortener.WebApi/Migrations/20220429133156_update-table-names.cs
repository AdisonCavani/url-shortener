using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.WebApi.Migrations
{
    public partial class updatetablenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomUrl_Users_UserId",
                table: "CustomUrl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Url",
                table: "Url");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomUrl",
                table: "CustomUrl");

            migrationBuilder.RenameTable(
                name: "Url",
                newName: "Urls");

            migrationBuilder.RenameTable(
                name: "CustomUrl",
                newName: "CustomUrls");

            migrationBuilder.RenameIndex(
                name: "IX_CustomUrl_UserId",
                table: "CustomUrls",
                newName: "IX_CustomUrls_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomUrls",
                table: "CustomUrls",
                column: "ShortUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomUrls_Users_UserId",
                table: "CustomUrls",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomUrls_Users_UserId",
                table: "CustomUrls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomUrls",
                table: "CustomUrls");

            migrationBuilder.RenameTable(
                name: "Urls",
                newName: "Url");

            migrationBuilder.RenameTable(
                name: "CustomUrls",
                newName: "CustomUrl");

            migrationBuilder.RenameIndex(
                name: "IX_CustomUrls_UserId",
                table: "CustomUrl",
                newName: "IX_CustomUrl_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Url",
                table: "Url",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomUrl",
                table: "CustomUrl",
                column: "ShortUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomUrl_Users_UserId",
                table: "CustomUrl",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
