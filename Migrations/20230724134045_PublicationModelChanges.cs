using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdeaExchange.Migrations
{
    /// <inheritdoc />
    public partial class PublicationModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
