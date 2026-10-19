using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class kek5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comment",
                table: "addresses");

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "suppliers_addresses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "customer_addresses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "flat_number",
                table: "addresses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "house_add1",
                table: "addresses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "house_add2",
                table: "addresses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "house_number",
                table: "addresses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "street_name",
                table: "addresses",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comment",
                table: "suppliers_addresses");

            migrationBuilder.DropColumn(
                name: "comment",
                table: "customer_addresses");

            migrationBuilder.DropColumn(
                name: "flat_number",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "house_add1",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "house_add2",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "house_number",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "street_name",
                table: "addresses");

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "addresses",
                type: "text",
                nullable: true);
        }
    }
}
