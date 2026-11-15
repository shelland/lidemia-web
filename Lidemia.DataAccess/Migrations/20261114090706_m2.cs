using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "average_rating_overall",
                table: "products");

            migrationBuilder.DropColumn(
                name: "average_rating_recent",
                table: "products");

            migrationBuilder.DropColumn(
                name: "total_rates",
                table: "products");

            migrationBuilder.AddColumn<long>(
                name: "review_stats_id",
                table: "products",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "product_review_stats",
                columns: table => new
                {
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    average_rating_recent = table.Column<double>(type: "double precision", nullable: true),
                    average_rating_overall = table.Column<double>(type: "double precision", nullable: true),
                    total_rates = table.Column<int>(type: "integer", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_review_stats", x => x.product_id);
                    table.ForeignKey(
                        name: "fk_product_review_stats_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_review_stats_is_active",
                table: "product_review_stats",
                column: "is_active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_review_stats");

            migrationBuilder.DropColumn(
                name: "review_stats_id",
                table: "products");

            migrationBuilder.AddColumn<double>(
                name: "average_rating_overall",
                table: "products",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "average_rating_recent",
                table: "products",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_rates",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
