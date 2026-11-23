using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "stats_id",
                table: "suppliers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "create_date",
                table: "supplier_stats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "supplier_stats",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "row_version",
                table: "supplier_stats",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "update_date",
                table: "supplier_stats",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_supplier_stats",
                table: "supplier_stats",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_stats_is_active",
                table: "supplier_stats",
                column: "is_active");

            migrationBuilder.AddForeignKey(
                name: "fk_supplier_stats_suppliers_supplier_id",
                table: "supplier_stats",
                column: "supplier_id",
                principalTable: "suppliers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_supplier_stats_suppliers_supplier_id",
                table: "supplier_stats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_supplier_stats",
                table: "supplier_stats");

            migrationBuilder.DropIndex(
                name: "ix_supplier_stats_is_active",
                table: "supplier_stats");

            migrationBuilder.DropColumn(
                name: "stats_id",
                table: "suppliers");

            migrationBuilder.DropColumn(
                name: "create_date",
                table: "supplier_stats");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "supplier_stats");

            migrationBuilder.DropColumn(
                name: "row_version",
                table: "supplier_stats");

            migrationBuilder.DropColumn(
                name: "update_date",
                table: "supplier_stats");
        }
    }
}
