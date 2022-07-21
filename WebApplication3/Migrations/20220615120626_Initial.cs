using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceAnalytic.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportanceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    NumericEquivalent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportanceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpendingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpendingCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(maxLength: 30, nullable: false),
                    Password = table.Column<string>(maxLength: 30, nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    UserRoleId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_UsersRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UsersRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsAchived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomeSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    IncomeCategoryId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeSubCategories_IncomeCategories_IncomeCategoryId",
                        column: x => x.IncomeCategoryId,
                        principalTable: "IncomeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeSubCategories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpendingSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    SpendingCategoryId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpendingSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpendingSubCategories_SpendingCategories_SpendingCategoryId",
                        column: x => x.SpendingCategoryId,
                        principalTable: "SpendingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpendingSubCategories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accumulations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    GoalId = table.Column<int>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: true),
                    SumInCurrency = table.Column<decimal>(nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accumulations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accumulations_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accumulations_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accumulations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id"
                        //onDelete: ReferentialAction.Cascade
                        );
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IncomeCategoryId = table.Column<int>(nullable: false),
                    IncomeSubCategoryId = table.Column<int>(nullable: true),
                    Sum = table.Column<decimal>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: true),
                    SumInCurrency = table.Column<decimal>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incomes_IncomeCategories_IncomeCategoryId",
                        column: x => x.IncomeCategoryId,
                        principalTable: "IncomeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incomes_IncomeSubCategories_IncomeSubCategoryId",
                        column: x => x.IncomeSubCategoryId,
                        principalTable: "IncomeSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incomes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanedSpendings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PeriodBegin = table.Column<DateTime>(nullable: false),
                    PeriodEnd = table.Column<DateTime>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: true),
                    SumInCurrency = table.Column<decimal>(nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: true),
                    SpendingSubCategoryId = table.Column<int>(nullable: true),
                    SpendingCategoryId = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanedSpendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanedSpendings_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanedSpendings_SpendingCategories_SpendingCategoryId",
                        column: x => x.SpendingCategoryId,
                        principalTable: "SpendingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanedSpendings_SpendingSubCategories_SpendingSubCategoryId",
                        column: x => x.SpendingSubCategoryId,
                        principalTable: "SpendingSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanedSpendings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spendings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SpendingCategoryId = table.Column<int>(nullable: false),
                    SpendingSubCategoryId = table.Column<int>(nullable: true),
                    Sum = table.Column<decimal>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: true),
                    SumInCurrency = table.Column<decimal>(nullable: true),
                    CurrencyRate = table.Column<decimal>(nullable: true),
                    ImportanceCategoryId = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spendings_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spendings_ImportanceCategories_ImportanceCategoryId",
                        column: x => x.ImportanceCategoryId,
                        principalTable: "ImportanceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spendings_SpendingCategories_SpendingCategoryId",
                        column: x => x.SpendingCategoryId,
                        principalTable: "SpendingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spendings_SpendingSubCategories_SpendingSubCategoryId",
                        column: x => x.SpendingSubCategoryId,
                        principalTable: "SpendingSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Spendings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "KGS" },
                    { 2, "USD" },
                    { 3, "EUR" },
                    { 4, "RUB" },
                    { 5, "KZT" }
                });

            migrationBuilder.InsertData(
                table: "ImportanceCategories",
                columns: new[] { "Id", "Name", "NumericEquivalent" },
                values: new object[,]
                {
                    { 1, "Не важно", 0 },
                    { 2, "Можно обойтись", 1 },
                    { 3, "Желательно", 2 },
                    { 4, "Необходимо", 3 },
                    { 5, "Крайне необходимо", 4 }
                });

            migrationBuilder.InsertData(
                table: "IncomeCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 7, "Подсобное хозяйство" },
                    { 6, "Предпринимательство" },
                    { 5, "Сдача в аренду" },
                    { 4, "Гос. выплаты" },
                    { 2, "Зарплата" },
                    { 1, "Другие источники" }
                });

            migrationBuilder.InsertData(
                table: "SpendingCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Другое" },
                    { 2, "Транспорт" },
                    { 3, "Оплата" },
                    { 4, "Покупки" },
                    { 5, "Продукты" },
                    { 6, "Питание вне дома" },
                    { 7, "Досуг" },
                    { 8, "На здоровье" }
                });

            migrationBuilder.InsertData(
                table: "UsersRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CurrencyId", "Login", "Password", "RegistrationDate", "UserRoleId" },
                values: new object[] { 1, 1, "admin", "admin123", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Accumulations_CurrencyId",
                table: "Accumulations",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accumulations_GoalId",
                table: "Accumulations",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_Accumulations_UserId",
                table: "Accumulations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                table: "Goals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_CurrencyId",
                table: "Incomes",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IncomeCategoryId",
                table: "Incomes",
                column: "IncomeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IncomeSubCategoryId",
                table: "Incomes",
                column: "IncomeSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_UserId",
                table: "Incomes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSubCategories_IncomeCategoryId",
                table: "IncomeSubCategories",
                column: "IncomeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSubCategories_UserId",
                table: "IncomeSubCategories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanedSpendings_CurrencyId",
                table: "PlanedSpendings",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanedSpendings_SpendingCategoryId",
                table: "PlanedSpendings",
                column: "SpendingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanedSpendings_SpendingSubCategoryId",
                table: "PlanedSpendings",
                column: "SpendingSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanedSpendings_UserId",
                table: "PlanedSpendings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_CurrencyId",
                table: "Spendings",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_ImportanceCategoryId",
                table: "Spendings",
                column: "ImportanceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_SpendingCategoryId",
                table: "Spendings",
                column: "SpendingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_SpendingSubCategoryId",
                table: "Spendings",
                column: "SpendingSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_UserId",
                table: "Spendings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpendingSubCategories_SpendingCategoryId",
                table: "SpendingSubCategories",
                column: "SpendingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SpendingSubCategories_UserId",
                table: "SpendingSubCategories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrencyId",
                table: "Users",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accumulations");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "PlanedSpendings");

            migrationBuilder.DropTable(
                name: "Spendings");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "IncomeSubCategories");

            migrationBuilder.DropTable(
                name: "ImportanceCategories");

            migrationBuilder.DropTable(
                name: "SpendingSubCategories");

            migrationBuilder.DropTable(
                name: "IncomeCategories");

            migrationBuilder.DropTable(
                name: "SpendingCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "UsersRoles");
        }
    }
}
