using Lidemia.Core.Models.Misc.Meta;
using Lidemia.DataAccess.Entities.Misc;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "products",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<ProductMetadata>(
                name: "metadata",
                table: "products",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.AddColumn<ProductSpecificationDbModel>(
                name: "specification",
                table: "products",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "metadata",
                table: "products");

            migrationBuilder.DropColumn(
                name: "specification",
                table: "products");

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
