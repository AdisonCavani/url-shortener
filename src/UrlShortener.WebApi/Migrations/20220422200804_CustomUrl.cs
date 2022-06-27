using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.WebApi.Migrations
{
    public partial class CustomUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUrl",
                columns: table => new
                {
                    ShortUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUrl", x => x.ShortUrl);
                    table.ForeignKey(
                        name: "FK_CustomUrl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomUrl_UserId",
                table: "CustomUrl",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUrl");
        }
    }
}
