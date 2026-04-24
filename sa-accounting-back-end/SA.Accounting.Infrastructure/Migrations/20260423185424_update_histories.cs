using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA.Accounting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_histories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "TransactionHistory",
                newName: "ToState");

            migrationBuilder.AddColumn<byte>(
                name: "CurrentState",
                table: "Transactions",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "FromState",
                table: "TransactionHistory",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TransactionHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FromState",
                table: "TransactionHistory");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TransactionHistory");

            migrationBuilder.RenameColumn(
                name: "ToState",
                table: "TransactionHistory",
                newName: "State");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
