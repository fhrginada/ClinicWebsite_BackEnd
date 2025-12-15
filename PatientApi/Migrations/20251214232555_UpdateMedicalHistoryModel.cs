using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalHistoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Medications",
                table: "MedicalHistories");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MedicalHistories",
                newName: "Treatment");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MedicalHistories",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MedicalHistories");

            migrationBuilder.RenameColumn(
                name: "Treatment",
                table: "MedicalHistories",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Medications",
                table: "MedicalHistories",
                type: "TEXT",
                nullable: true);
        }
    }
}
