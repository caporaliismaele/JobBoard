using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Migrations
{
    /// <inheritdoc />
    public partial class RenameCandidateProfileColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cognome",
                table: "CandidateProfiles");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "CandidateProfiles");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "CandidateProfiles",
                newName: "Telephone");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CandidateProfiles",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "CandidateProfiles",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CandidateProfiles");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "CandidateProfiles");

            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "CandidateProfiles",
                newName: "Telefono");

            migrationBuilder.AddColumn<string>(
                name: "Cognome",
                table: "CandidateProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "CandidateProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
