using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftitoFlix.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "IMDBrating",
                table: "Medias",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMDBrating",
                table: "Medias");
        }
    }
}
