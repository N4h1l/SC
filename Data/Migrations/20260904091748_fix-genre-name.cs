using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SC.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixgenrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Gerne_GenreId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gerne",
                table: "Gerne");

            migrationBuilder.RenameTable(
                name: "Gerne",
                newName: "Genre");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                table: "Genre",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Genre_GenreId",
                table: "Book",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Genre_GenreId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                table: "Genre");

            migrationBuilder.RenameTable(
                name: "Genre",
                newName: "Gerne");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gerne",
                table: "Gerne",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Gerne_GenreId",
                table: "Book",
                column: "GenreId",
                principalTable: "Gerne",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
