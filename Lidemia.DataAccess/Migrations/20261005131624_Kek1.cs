using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Kek1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_suppliers_addresses",
                table: "suppliers_addresses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_addresses",
                table: "customer_addresses");

            migrationBuilder.DropColumn(
                name: "city",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "region",
                table: "addresses");

            migrationBuilder.AlterColumn<int>(
                name: "row_version",
                table: "suppliers_addresses",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "suppliers_addresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "suppliers_addresses",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "suppliers_addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "available_units",
                table: "products",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "orders",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "customer_address_id",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "delivery_type",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "supplier_address_id",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "row_version",
                table: "customer_addresses",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "customer_addresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "customer_addresses",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuidv7()");

            migrationBuilder.AlterColumn<int>(
                name: "row_version",
                table: "addresses",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "addresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "city_id",
                table: "addresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "addresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "country_id",
                table: "addresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "region_id",
                table: "addresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_suppliers_addresses",
                table: "suppliers_addresses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_addresses",
                table: "customer_addresses",
                column: "id");

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    country_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regions", x => x.id);
                    table.ForeignKey(
                        name: "fk_regions_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    region_id = table.Column<Guid>(type: "uuid", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.id);
                    table.ForeignKey(
                        name: "fk_cities_regions_region_id",
                        column: x => x.region_id,
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_addresses_is_active",
                table: "suppliers_addresses",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_orders_customer_address_id",
                table: "orders",
                column: "customer_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_supplier_address_id",
                table: "orders",
                column: "supplier_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_addresses_is_active",
                table: "customer_addresses",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_addresses_city_id",
                table: "addresses",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_addresses_country_id",
                table: "addresses",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_addresses_is_active",
                table: "addresses",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_addresses_region_id",
                table: "addresses",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_cities_is_active",
                table: "cities",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_cities_region_id",
                table: "cities",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_countries_is_active",
                table: "countries",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_regions_country_id",
                table: "regions",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_regions_is_active",
                table: "regions",
                column: "is_active");

            migrationBuilder.AddForeignKey(
                name: "fk_addresses_cities_city_id",
                table: "addresses",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_addresses_countries_country_id",
                table: "addresses",
                column: "country_id",
                principalTable: "countries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_addresses_regions_region_id",
                table: "addresses",
                column: "region_id",
                principalTable: "regions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_customer_addresses_customer_address_id",
                table: "orders",
                column: "customer_address_id",
                principalTable: "customer_addresses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_suppliers_addresses_supplier_address_id",
                table: "orders",
                column: "supplier_address_id",
                principalTable: "suppliers_addresses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_addresses_cities_city_id",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "fk_addresses_countries_country_id",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "fk_addresses_regions_region_id",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_customer_addresses_customer_address_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_suppliers_addresses_supplier_address_id",
                table: "orders");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "regions");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropPrimaryKey(
                name: "pk_suppliers_addresses",
                table: "suppliers_addresses");

            migrationBuilder.DropIndex(
                name: "ix_suppliers_addresses_is_active",
                table: "suppliers_addresses");

            migrationBuilder.DropIndex(
                name: "ix_orders_customer_address_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_supplier_address_id",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_addresses",
                table: "customer_addresses");

            migrationBuilder.DropIndex(
                name: "ix_customer_addresses_is_active",
                table: "customer_addresses");

            migrationBuilder.DropIndex(
                name: "ix_addresses_city_id",
                table: "addresses");

            migrationBuilder.DropIndex(
                name: "ix_addresses_country_id",
                table: "addresses");

            migrationBuilder.DropIndex(
                name: "ix_addresses_is_active",
                table: "addresses");

            migrationBuilder.DropIndex(
                name: "ix_addresses_region_id",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "id",
                table: "suppliers_addresses");

            migrationBuilder.DropColumn(
                name: "type",
                table: "suppliers_addresses");

            migrationBuilder.DropColumn(
                name: "available_units",
                table: "products");

            migrationBuilder.DropColumn(
                name: "customer_address_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "delivery_type",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "supplier_address_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "id",
                table: "customer_addresses");

            migrationBuilder.DropColumn(
                name: "city_id",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "comment",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "country_id",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "addresses");

            migrationBuilder.AlterColumn<int>(
                name: "row_version",
                table: "suppliers_addresses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "suppliers_addresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "row_version",
                table: "customer_addresses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "customer_addresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<int>(
                name: "row_version",
                table: "addresses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "addresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "addresses",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "region",
                table: "addresses",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_suppliers_addresses",
                table: "suppliers_addresses",
                columns: new[] { "supplier_id", "address_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_addresses",
                table: "customer_addresses",
                columns: new[] { "customer_id", "address_id" });
        }
    }
}
