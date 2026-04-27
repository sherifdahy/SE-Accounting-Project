using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA.Accounting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_relation_between_application_user_and_permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_AspNetUsers_ApplicationUserId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_ApplicationUserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Permissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_AspNetUsers_UserId",
                table: "Permissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_AspNetUsers_UserId",
                table: "Permissions");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ApplicationUserId",
                table: "Permissions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_AspNetUsers_ApplicationUserId",
                table: "Permissions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
