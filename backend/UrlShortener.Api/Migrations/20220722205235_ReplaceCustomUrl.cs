using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UrlShortener.Api.Migrations
{
    public partial class ReplaceCustomUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUrls");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Urls",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AnonymousUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousUrls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnonymousUrls");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Urls");

            migrationBuilder.CreateTable(
                name: "CustomUrls",
                columns: table => new
                {
                    ShortUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUrls", x => x.ShortUrl);
                });
        }
    }
}
