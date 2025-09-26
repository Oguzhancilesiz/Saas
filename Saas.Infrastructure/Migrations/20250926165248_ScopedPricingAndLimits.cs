using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ScopedPricingAndLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppPlanPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BillingPeriod = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveFromUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlanPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TrialDays = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureLimits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: false),
                    LimitPerPeriod = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EffectiveFromUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    BillingPeriod = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveFromUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AppPlanPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FromQuantity = table.Column<int>(type: "int", nullable: false),
                    ToQuantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTiers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlanPrices_AppId_PlanId_BillingPeriod_EffectiveFromUtc_EffectiveToUtc",
                table: "AppPlanPrices",
                columns: new[] { "AppId", "PlanId", "BillingPeriod", "EffectiveFromUtc", "EffectiveToUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlans_AppId_PlanId",
                table: "AppPlans",
                columns: new[] { "AppId", "PlanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureLimits_FeatureId_AppId_PlanId_UserId_Period_EffectiveFromUtc_EffectiveToUtc",
                table: "FeatureLimits",
                columns: new[] { "FeatureId", "AppId", "PlanId", "UserId", "Period", "EffectiveFromUtc", "EffectiveToUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanPrices_PlanId_BillingPeriod_EffectiveFromUtc_EffectiveToUtc",
                table: "PlanPrices",
                columns: new[] { "PlanId", "BillingPeriod", "EffectiveFromUtc", "EffectiveToUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceTiers_PlanPriceId_AppPlanPriceId_FromQuantity_ToQuantity",
                table: "PriceTiers",
                columns: new[] { "PlanPriceId", "AppPlanPriceId", "FromQuantity", "ToQuantity" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPlanPrices");

            migrationBuilder.DropTable(
                name: "AppPlans");

            migrationBuilder.DropTable(
                name: "FeatureLimits");

            migrationBuilder.DropTable(
                name: "PlanPrices");

            migrationBuilder.DropTable(
                name: "PriceTiers");
        }
    }
}
