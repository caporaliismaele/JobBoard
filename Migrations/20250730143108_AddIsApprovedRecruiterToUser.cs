using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedRecruiterToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatures_AspNetUsers_CandidateId",
                table: "Candidatures");

            migrationBuilder.DropIndex(
                name: "IX_CandidateProfiles_UserId",
                table: "CandidateProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedRecruiter",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CandidateProfiles_UserId",
                table: "CandidateProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatures_CandidateProfiles_CandidateId",
                table: "Candidatures",
                column: "CandidateId",
                principalTable: "CandidateProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatures_CandidateProfiles_CandidateId",
                table: "Candidatures");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CandidateProfiles_UserId",
                table: "CandidateProfiles");

            migrationBuilder.DropColumn(
                name: "IsApprovedRecruiter",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateProfiles_UserId",
                table: "CandidateProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatures_AspNetUsers_CandidateId",
                table: "Candidatures",
                column: "CandidateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
