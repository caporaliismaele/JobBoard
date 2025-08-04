using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Migrations
{
    /// <inheritdoc />
    public partial class RenameJobOfferColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titolo",
                table: "JobOffers",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Descrizione",
                table: "JobOffers",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DataPubblicazione",
                table: "JobOffers",
                newName: "PublishedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "JobOffers",
                newName: "Titolo");

            migrationBuilder.RenameColumn(
                name: "PublishedDate",
                table: "JobOffers",
                newName: "DataPubblicazione");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "JobOffers",
                newName: "Descrizione");
        }
    }
}
