using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class kek4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_products_is_visible",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "ix_products_is_visible_is_active",
                table: "products",
                columns: new[] { "is_visible", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_products_is_visible_is_active",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "ix_products_is_visible",
                table: "products",
                column: "is_visible");
        }
    }
}
