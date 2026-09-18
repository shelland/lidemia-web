using System;
using System.Collections.Generic;
using Lidemia.DataAccess.Entities.Meta;
using Lidemia.DataAccess.Entities.Misc;
using Lidemia.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.PreInit();

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "auth_tokens",
                columns: table => new
                {
                    access_token = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    refresh_token = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    expiration_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    entity_id = table.Column<long>(type: "bigint", nullable: false),
                    entity_type = table.Column<int>(type: "integer", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    name_key = table.Column<string>(type: "text", nullable: false),
                    description_key = table.Column<string>(type: "text", nullable: true),
                    image_class = table.Column<string>(type: "text", nullable: true),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_categories_product_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "product_categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "suppliers_stats",
                columns: table => new
                {
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    total_orders = table.Column<int>(type: "integer", nullable: false),
                    average_rating = table.Column<double>(type: "double precision", nullable: false),
                    total_products = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email_normalized = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password_salt = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    logo_url = table.Column<string>(type: "text", nullable: true),
                    tin = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<CustomerMetadata>(type: "jsonb", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "password_resets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    entity_type = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_password_resets", x => x.id);
                    table.ForeignKey(
                        name: "fk_password_resets_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    large_thumb_url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    medium_thumb_url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    small_thumb_url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    extra_path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    is_processed = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_photos_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shopping_carts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    items = table.Column<IEnumerable<ShoppingCartItemDbModel>>(type: "jsonb", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shopping_carts", x => x.id);
                    table.ForeignKey(
                        name: "fk_shopping_carts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    short_description = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    logo_url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    cover_url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    inn = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    ogrn = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    organization_type = table.Column<int>(type: "integer", nullable: false),
                    legal_address = table.Column<string>(type: "text", nullable: true),
                    actual_address = table.Column<string>(type: "text", nullable: true),
                    contact_person_name = table.Column<string>(type: "text", nullable: true),
                    contact_person_role = table.Column<string>(type: "text", nullable: true),
                    is_email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    metadata = table.Column<SupplierMetadata>(type: "jsonb", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => x.id);
                    table.ForeignKey(
                        name: "fk_suppliers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    postal_index = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    region = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    city = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address_line1 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    address_line2 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_addresses_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wish_lists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    items_count = table.Column<int>(type: "integer", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    short_description = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    primary_photo_id = table.Column<long>(type: "bigint", nullable: true),
                    is_discount = table.Column<bool>(type: "boolean", nullable: false),
                    has_current_discount = table.Column<bool>(type: "boolean", nullable: false),
                    discount_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    discount_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    discount_price = table.Column<decimal>(type: "numeric", nullable: true),
                    min_quantity = table.Column<double>(type: "double precision", nullable: true),
                    max_quantity = table.Column<double>(type: "double precision", nullable: true),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: true),
                    average_rating_recent = table.Column<double>(type: "double precision", nullable: true),
                    average_rating_overall = table.Column<double>(type: "double precision", nullable: true),
                    total_rates = table.Column<int>(type: "integer", nullable: false),
                    first_seen_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    price_unit = table.Column<int>(type: "integer", nullable: true),
                    availability_type = table.Column<int>(type: "integer", nullable: true),
                    order_rank = table.Column<int>(type: "integer", nullable: true),
                    sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_photos_primary_photo_id",
                        column: x => x.primary_photo_id,
                        principalTable: "photos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_products_product_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "product_categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_products_products_parent_id",
                        column: x => x.parent_id,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_products_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promo_codes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    code = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    starts_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    max_usages = table.Column<int>(type: "integer", nullable: true),
                    max_allowed_total_sum = table.Column<decimal>(type: "numeric", nullable: true),
                    is_one_time = table.Column<bool>(type: "boolean", nullable: false),
                    total_usages = table.Column<int>(type: "integer", nullable: false),
                    allowed_products = table.Column<long[]>(type: "bigint[]", nullable: false),
                    allowed_customers = table.Column<long[]>(type: "bigint[]", nullable: false),
                    allowed_categories = table.Column<long[]>(type: "bigint[]", nullable: false),
                    discount_type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promo_codes", x => x.id);
                    table.ForeignKey(
                        name: "fk_promo_codes_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_addresses",
                columns: table => new
                {
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    address_id = table.Column<long>(type: "bigint", nullable: false),
                    address_id1 = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_addresses", x => new { x.customer_id, x.address_id });
                    table.ForeignKey(
                        name: "fk_customer_addresses_addresses_address_id1",
                        column: x => x.address_id1,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_customer_addresses_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "suppliers_addresses",
                columns: table => new
                {
                    supplier_id = table.Column<long>(type: "bigint", nullable: false),
                    address_id = table.Column<long>(type: "bigint", nullable: false),
                    address_id1 = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers_addresses", x => new { x.supplier_id, x.address_id });
                    table.ForeignKey(
                        name: "fk_suppliers_addresses_addresses_address_id1",
                        column: x => x.address_id1,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_suppliers_addresses_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_feedbacks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    comment = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    rate = table.Column<byte>(type: "smallint", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_feedbacks_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_feedbacks_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_products_photos_photos_photo_id",
                        column: x => x.photo_id,
                        principalTable: "photos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_products_photos_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wish_list_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    wish_list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wish_list_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_wish_list_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_wish_list_items_wish_lists_wish_list_id",
                        column: x => x.wish_list_id,
                        principalTable: "wish_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "service.next_id()"),
                    number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    is_finish_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    last_status_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    metadata = table.Column<OrderMetadata>(type: "jsonb", nullable: false),
                    promo_code_id = table.Column<long>(type: "bigint", nullable: true),
                    row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_orders_promo_codes_promo_code_id",
                        column: x => x.promo_code_id,
                        principalTable: "promo_codes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    row_version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_addresses_customer_id_is_active",
                table: "addresses",
                columns: new[] { "customer_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_addresses_customer_id_is_default_is_active",
                table: "addresses",
                columns: new[] { "customer_id", "is_default", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_auth_tokens_access_token_is_active",
                table: "auth_tokens",
                columns: new[] { "access_token", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_auth_tokens_entity_id_is_active",
                table: "auth_tokens",
                columns: new[] { "entity_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_auth_tokens_is_active",
                table: "auth_tokens",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_customer_addresses_address_id1",
                table: "customer_addresses",
                column: "address_id1");

            migrationBuilder.CreateIndex(
                name: "ix_customer_addresses_customer_id_is_active",
                table: "customer_addresses",
                columns: new[] { "customer_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_customers_is_active",
                table: "customers",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_customers_user_id",
                table: "customers",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id_is_active",
                table: "order_items",
                columns: new[] { "order_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_order_items_product_id_is_active",
                table: "order_items",
                columns: new[] { "product_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_orders_customer_id_is_active",
                table: "orders",
                columns: new[] { "customer_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_orders_is_active",
                table: "orders",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_orders_promo_code_id",
                table: "orders",
                column: "promo_code_id");

            migrationBuilder.CreateIndex(
                name: "ix_password_resets_code",
                table: "password_resets",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_password_resets_user_id",
                table: "password_resets",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_photos_is_active",
                table: "photos",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_photos_user_id",
                table: "photos",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_categories_is_active",
                table: "product_categories",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_product_categories_parent_id",
                table: "product_categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_feedbacks_customer_id_is_active",
                table: "product_feedbacks",
                columns: new[] { "customer_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_product_feedbacks_product_id_is_active",
                table: "product_feedbacks",
                columns: new[] { "product_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_product_feedbacks_product_id_state_is_active",
                table: "product_feedbacks",
                columns: new[] { "product_id", "state", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_is_active",
                table: "products",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_products_is_visible",
                table: "products",
                column: "is_visible");

            migrationBuilder.CreateIndex(
                name: "ix_products_parent_id",
                table: "products",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_primary_photo_id",
                table: "products",
                column: "primary_photo_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_supplier_id",
                table: "products",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_photos_photo_id",
                table: "products_photos",
                column: "photo_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_photos_product_id",
                table: "products_photos",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_promo_codes_is_active",
                table: "promo_codes",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_promo_codes_supplier_id",
                table: "promo_codes",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "ix_shopping_carts_user_id_is_active",
                table: "shopping_carts",
                columns: new[] { "user_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_is_active",
                table: "suppliers",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_user_id",
                table: "suppliers",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_addresses_address_id1",
                table: "suppliers_addresses",
                column: "address_id1");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_addresses_supplier_id_is_active",
                table: "suppliers_addresses",
                columns: new[] { "supplier_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_email_normalized",
                table: "users",
                column: "email_normalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_is_active",
                table: "users",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_users_phone",
                table: "users",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_wish_list_items_product_id",
                table: "wish_list_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_wish_list_items_wish_list_id_is_active",
                table: "wish_list_items",
                columns: new[] { "wish_list_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_wish_list_items_wish_list_id_product_id",
                table: "wish_list_items",
                columns: new[] { "wish_list_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_wish_lists_customer_id_is_active",
                table: "wish_lists",
                columns: new[] { "customer_id", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_tokens");

            migrationBuilder.DropTable(
                name: "customer_addresses");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "password_resets");

            migrationBuilder.DropTable(
                name: "product_feedbacks");

            migrationBuilder.DropTable(
                name: "products_photos");

            migrationBuilder.DropTable(
                name: "shopping_carts");

            migrationBuilder.DropTable(
                name: "suppliers_addresses");

            migrationBuilder.DropTable(
                name: "suppliers_stats");

            migrationBuilder.DropTable(
                name: "wish_list_items");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "wish_lists");

            migrationBuilder.DropTable(
                name: "promo_codes");

            migrationBuilder.DropTable(
                name: "photos");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
