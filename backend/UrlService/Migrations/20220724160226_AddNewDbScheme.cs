using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UrlService.Migrations
{
    public partial class AddNewDbScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUrls");

            migrationBuilder.AddColumn<long>(
                name: "UrlDetailsId",
                table: "Urls",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UrlsDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlsDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UrlDetailsId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_UrlsDetails_UrlDetailsId",
                        column: x => x.UrlDetailsId,
                        principalTable: "UrlsDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UrlDetailsId",
                table: "Urls",
                column: "UrlDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UrlDetailsId",
                table: "Tags",
                column: "UrlDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_UrlsDetails_UrlDetailsId",
                table: "Urls",
                column: "UrlDetailsId",
                principalTable: "UrlsDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_UrlsDetails_UrlDetailsId",
                table: "Urls");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "UrlsDetails");

            migrationBuilder.DropIndex(
                name: "IX_Urls_UrlDetailsId",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "UrlDetailsId",
                table: "Urls");

            migrationBuilder.CreateTable(
                name: "UserUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUrls", x => x.Id);
                });
        }
    }
}
