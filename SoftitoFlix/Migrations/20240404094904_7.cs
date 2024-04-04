using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftitoFlix.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMDB_Rating",
                table: "Medias");

            migrationBuilder.AddColumn<long>(
                name: "RatedBy",
                table: "Medias",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Medias",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatedBy",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Medias");

            migrationBuilder.AddColumn<float>(
                name: "IMDB_Rating",
                table: "Medias",
                type: "real",
                nullable: true);
        }
    }
}
