using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lidemia.DataAccess.Migrations;

/// <inheritdoc />
public partial class m7 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "chats",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                customer_id = table.Column<long>(type: "bigint", nullable: false),
                supplier_id = table.Column<long>(type: "bigint", nullable: false),
                last_message_text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                last_message_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_chats", x => x.id);
                table.ForeignKey(
                    name: "fk_chats_customers_customer_id",
                    column: x => x.customer_id,
                    principalTable: "customers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_chats_suppliers_supplier_id",
                    column: x => x.supplier_id,
                    principalTable: "suppliers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "chat_messages",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                chat_id = table.Column<long>(type: "bigint", nullable: false),
                sender_type = table.Column<int>(type: "integer", nullable: false),
                sender_id = table.Column<long>(type: "bigint", nullable: false),
                text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                is_read = table.Column<bool>(type: "boolean", nullable: false),
                read_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                row_version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                create_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                update_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_chat_messages", x => x.id);
                table.ForeignKey(
                    name: "fk_chat_messages_chats_chat_id",
                    column: x => x.chat_id,
                    principalTable: "chats",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_chat_messages_chat_id_create_date",
            table: "chat_messages",
            columns: new[] { "chat_id", "create_date" });

        migrationBuilder.CreateIndex(
            name: "ix_chat_messages_chat_id_is_active",
            table: "chat_messages",
            columns: new[] { "chat_id", "is_active" });

        migrationBuilder.CreateIndex(
            name: "ix_chat_messages_chat_id_is_read",
            table: "chat_messages",
            columns: new[] { "chat_id", "is_read" });

        migrationBuilder.CreateIndex(
            name: "ix_chat_messages_is_active",
            table: "chat_messages",
            column: "is_active");

        migrationBuilder.CreateIndex(
            name: "ix_chats_customer_id_is_active",
            table: "chats",
            columns: new[] { "customer_id", "is_active" });

        migrationBuilder.CreateIndex(
            name: "ix_chats_customer_id_supplier_id",
            table: "chats",
            columns: new[] { "customer_id", "supplier_id" },
            unique: true,
            filter: "\"is_active\"");

        migrationBuilder.CreateIndex(
            name: "ix_chats_is_active",
            table: "chats",
            column: "is_active");

        migrationBuilder.CreateIndex(
            name: "ix_chats_supplier_id_is_active",
            table: "chats",
            columns: new[] { "supplier_id", "is_active" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "chat_messages");

        migrationBuilder.DropTable(
            name: "chats");
    }
}
