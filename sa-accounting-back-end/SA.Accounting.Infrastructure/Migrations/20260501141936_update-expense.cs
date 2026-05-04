using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SA.Accounting.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateexpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funds");

            migrationBuilder.DropTable(
                name: "TransactionHistory");

            migrationBuilder.DropTable(
                name: "TransactionItems");

            migrationBuilder.DropTable(
                name: "TransactionCategories");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.CreateTable(
                name: "Custodies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custodies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Custodies_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Custodies_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Custodies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RequiresAttachment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                    table.CheckConstraint("CK_ExpenseCategory_Name_NotEmpty", "LEN(LTRIM(RTRIM([Name]))) > 0");
                    table.ForeignKey(
                        name: "FK_ExpenseCategories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseCategories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseClaims", x => x.Id);
                    table.CheckConstraint("CK_ExpenseClaim_Number_NotEmpty", "LEN(LTRIM(RTRIM([Number]))) > 0");
                    table.CheckConstraint("CK_ExpenseClaim_State_Valid", "[CurrentState] IN (1, 2, 3, 4, 5, 6, 7, 8, 9)");
                    table.ForeignKey(
                        name: "FK_ExpenseClaims_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseClaims_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseClaimHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseClaimId = table.Column<int>(type: "int", nullable: false),
                    FromState = table.Column<int>(type: "int", nullable: false),
                    ToState = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseClaimHistories", x => x.Id);
                    table.CheckConstraint("CK_ExpenseClaimHistory_FromState_NotEqual_ToState", "[FromState] <> [ToState]");
                    table.CheckConstraint("CK_ExpenseClaimHistory_FromState_Valid", "[FromState] IN (1, 2, 3, 4, 5, 6, 7, 8, 9)");
                    table.CheckConstraint("CK_ExpenseClaimHistory_ToState_Valid", "[ToState] IN (1, 2, 3, 4, 5, 6, 7, 8, 9)");
                    table.ForeignKey(
                        name: "FK_ExpenseClaimHistories_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExpenseClaimHistories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseClaimHistories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExpenseClaimHistories_ExpenseClaims_ExpenseClaimId",
                        column: x => x.ExpenseClaimId,
                        principalTable: "ExpenseClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseClaimItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExpenseClaimId = table.Column<int>(type: "int", nullable: false),
                    ExpenseCategoryId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseClaimItems", x => x.Id);
                    table.CheckConstraint("CK_ExpenseClaimItem_Amount_Positive", "[Amount] > 0");
                    table.CheckConstraint("CK_ExpenseClaimItem_RejectionReason_Required_WhenRejected", "(\r\n                [State] <> 3\r\n                OR\r\n                (\r\n                    [State] = 3 \r\n                    AND [RejectionReason] IS NOT NULL \r\n                    AND LEN(LTRIM(RTRIM([RejectionReason]))) > 0\r\n                )\r\n            )");
                    table.CheckConstraint("CK_ExpenseClaimItem_State_Valid", "[State] IN (1, 2, 3)");
                    table.ForeignKey(
                        name: "FK_ExpenseClaimItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseClaimItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExpenseClaimItems_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseClaimItems_ExpenseCategories_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseClaimItems_ExpenseClaims_ExpenseClaimId",
                        column: x => x.ExpenseClaimId,
                        principalTable: "ExpenseClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustodyId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpenseClaimId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                    table.CheckConstraint("CK_Movement_Amount_Positive", "[Amount] > 0");
                    table.CheckConstraint("CK_Movement_ExpenseClaim_Rule", "(\r\n                ([Type] = 2 AND [ExpenseClaimId] IS NOT NULL)\r\n                OR\r\n                ([Type] <> 2 AND [ExpenseClaimId] IS NULL)\r\n            )");
                    table.CheckConstraint("CK_Movement_Type_Valid", "[Type] IN (1, 2, 3, 4, 5)");
                    table.ForeignKey(
                        name: "FK_Movements_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movements_Custodies_CustodyId",
                        column: x => x.CustodyId,
                        principalTable: "Custodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movements_ExpenseClaims_ExpenseClaimId",
                        column: x => x.ExpenseClaimId,
                        principalTable: "ExpenseClaims",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Custodies_CreatedById",
                table: "Custodies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Custodies_Number",
                table: "Custodies",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Custodies_UpdatedById",
                table: "Custodies",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Custodies_UserId",
                table: "Custodies",
                column: "UserId",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategories_CreatedById",
                table: "ExpenseCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategories_Name",
                table: "ExpenseCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategories_UpdatedById",
                table: "ExpenseCategories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimHistories_ApplicationUserId",
                table: "ExpenseClaimHistories",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimHistories_CreatedById",
                table: "ExpenseClaimHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimHistories_ExpenseClaimId",
                table: "ExpenseClaimHistories",
                column: "ExpenseClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimHistories_UpdatedById",
                table: "ExpenseClaimHistories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimItems_CompanyId",
                table: "ExpenseClaimItems",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimItems_CreatedById",
                table: "ExpenseClaimItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimItems_ExpenseCategoryId",
                table: "ExpenseClaimItems",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimItems_ExpenseClaimId",
                table: "ExpenseClaimItems",
                column: "ExpenseClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaimItems_UpdatedById",
                table: "ExpenseClaimItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaims_CreatedById",
                table: "ExpenseClaims",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaims_Number",
                table: "ExpenseClaims",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaims_UpdatedById",
                table: "ExpenseClaims",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseClaims_UserId_DateTime",
                table: "ExpenseClaims",
                columns: new[] { "UserId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Movements_CreatedById",
                table: "Movements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_CustodyId",
                table: "Movements",
                column: "CustodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_ExpenseClaimId_Type",
                table: "Movements",
                columns: new[] { "ExpenseClaimId", "Type" },
                unique: true,
                filter: "[ExpenseClaimId] IS NOT NULL AND [Type] = 2");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_UpdatedById",
                table: "Movements",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseClaimHistories");

            migrationBuilder.DropTable(
                name: "ExpenseClaimItems");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropTable(
                name: "Custodies");

            migrationBuilder.DropTable(
                name: "ExpenseClaims");

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funds_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Funds_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Funds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionCategories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionCategories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentState = table.Column<byte>(type: "tinyint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromState = table.Column<byte>(type: "tinyint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToState = table.Column<byte>(type: "tinyint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionHistory_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionHistory_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionHistory_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    TransactionCategoryId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionItems_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionItems_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionItems_TransactionCategories_TransactionCategoryId",
                        column: x => x.TransactionCategoryId,
                        principalTable: "TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionItems_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funds_CreatedById",
                table: "Funds",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Funds_UpdatedById",
                table: "Funds",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Funds_UserId",
                table: "Funds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategories_CreatedById",
                table: "TransactionCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategories_UpdatedById",
                table: "TransactionCategories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_CreatedById",
                table: "TransactionHistory",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_TransactionId",
                table: "TransactionHistory",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_UpdatedById",
                table: "TransactionHistory",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_CompanyId",
                table: "TransactionItems",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_CreatedById",
                table: "TransactionItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_TransactionCategoryId",
                table: "TransactionItems",
                column: "TransactionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_TransactionId",
                table: "TransactionItems",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_UpdatedById",
                table: "TransactionItems",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedById",
                table: "Transactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UpdatedById",
                table: "Transactions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }
    }
}
