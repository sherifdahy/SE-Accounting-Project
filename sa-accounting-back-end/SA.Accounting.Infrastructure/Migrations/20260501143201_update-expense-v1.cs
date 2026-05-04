using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA.Accounting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateexpensev1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "ExpenseClaims",
                newName: "ClaimDate");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseClaims_UserId_DateTime",
                table: "ExpenseClaims",
                newName: "IX_ExpenseClaims_UserId_ClaimDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClaimDate",
                table: "ExpenseClaims",
                newName: "DateTime");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseClaims_UserId_ClaimDate",
                table: "ExpenseClaims",
                newName: "IX_ExpenseClaims_UserId_DateTime");
        }
    }
}
