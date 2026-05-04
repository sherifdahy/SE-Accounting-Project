using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA.Accounting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateexpensecategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpenseCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "ExpenseCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "ExpenseCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpenseCategories",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
