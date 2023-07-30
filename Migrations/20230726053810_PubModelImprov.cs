using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdeaExchange.Migrations
{
    /// <inheritdoc />
    public partial class PubModelImprov : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Publications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Publications");
        }
    }
}
