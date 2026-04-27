using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA.Accounting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_table_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_AspNetUsers_UserId",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "DeniedPermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeniedPermissions",
                table: "DeniedPermissions",
                columns: new[] { "UserId", "Value" });

            migrationBuilder.AddForeignKey(
                name: "FK_DeniedPermissions_AspNetUsers_UserId",
                table: "DeniedPermissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeniedPermissions_AspNetUsers_UserId",
                table: "DeniedPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeniedPermissions",
                table: "DeniedPermissions");

            migrationBuilder.RenameTable(
                name: "DeniedPermissions",
                newName: "Permissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                columns: new[] { "UserId", "Value" });

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_AspNetUsers_UserId",
                table: "Permissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
