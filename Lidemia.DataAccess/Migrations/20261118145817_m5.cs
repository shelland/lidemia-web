using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "total_sum",
                table: "orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_sum_discount",
                table: "orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "order_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<int>(
                name: "discount_source",
                table: "order_items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price_per_item",
                table: "order_items",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "price_per_item_discount",
                table: "order_items",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "promo_code_id",
                table: "order_items",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_promo_code_id",
                table: "order_items",
                column: "promo_code_id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_items_promo_codes_promo_code_id",
                table: "order_items",
                column: "promo_code_id",
                principalTable: "promo_codes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_items_promo_codes_promo_code_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "ix_order_items_promo_code_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "total_sum",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "total_sum_discount",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "discount_source",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "price_per_item",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "price_per_item_discount",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "promo_code_id",
                table: "order_items");

            migrationBuilder.AlterColumn<double>(
                name: "quantity",
                table: "order_items",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
