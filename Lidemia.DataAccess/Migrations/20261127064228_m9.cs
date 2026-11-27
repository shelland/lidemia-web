using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lidemia.DataAccess.Migrations;

/// <inheritdoc />
public partial class _20261127064228_m9 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_wish_list_items_wish_lists_wish_list_id",
            table: "wish_list_items");

        migrationBuilder.DropTable(
            name: "wish_lists");

        migrationBuilder.DropIndex(
            name: "ix_wish_list_items_wish_list_id_product_id",
            table: "wish_list_items");

        migrationBuilder.RenameColumn(
            name: "wish_list_id",
            table: "wish_list_items",
            newName: "customer_id");

        migrationBuilder.RenameIndex(
            name: "ix_wish_list_items_wish_list_id_is_active",
            table: "wish_list_items",
            newName: "ix_wish_list_items_customer_id_is_active");

        migrationBuilder.AddForeignKey(
            name: "fk_wish_list_items_customers_customer_id",
            table: "wish_list_items",
            column: "customer_id",
            principalTable: "customers",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_wish_list_items_customers_customer_id",
            table: "wish_list_items");

        migrationBuilder.RenameColumn(
            name: "customer_id",
            table: "wish_list_items",
            newName: "wish_list_id");

        migrationBuilder.RenameIndex(
            name: "ix_wish_list_items_customer_id_is_active",
            table: "wish_list_items",
            newName: "ix_wish_list_items_wish_list_id_is_active");

        migrationBuilder.CreateTable(
            name: "wish_lists",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                customer_id = table.Column<long>(type: "bigint", nullable: false),
                create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                items_count = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                row_version = table.Column<int>(type: "integer", nullable: false),
                update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_wish_lists", x => x.id);
                table.ForeignKey(
                    name: "fk_wish_lists_customers_customer_id",
                    column: x => x.customer_id,
                    principalTable: "customers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_wish_list_items_wish_list_id_product_id",
            table: "wish_list_items",
            columns: new[] { "wish_list_id", "product_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_wish_lists_customer_id_is_active",
            table: "wish_lists",
            columns: new[] { "customer_id", "is_active" });

        migrationBuilder.AddForeignKey(
            name: "fk_wish_list_items_wish_lists_wish_list_id",
            table: "wish_list_items",
            column: "wish_list_id",
            principalTable: "wish_lists",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}
