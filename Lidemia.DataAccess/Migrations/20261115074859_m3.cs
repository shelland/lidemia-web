using Lidemia.DataAccess.Entities.Misc;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ProductReviewRateDistributionDbModel>(
                name: "distribution",
                table: "product_review_stats",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "distribution",
                table: "product_review_stats");
        }
    }
}
