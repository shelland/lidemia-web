using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class kek2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "tags",
                table: "products",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tags",
                table: "products");
        }
    }
}
