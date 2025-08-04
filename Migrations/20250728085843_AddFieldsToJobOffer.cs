using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToJobOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptPrivacyPolicy",
                table: "CandidateProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "JobOffers");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptPrivacyPolicy",
                table: "CandidateProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
