using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lidemia.DataAccess.Migrations;

/// <inheritdoc />
public partial class m8 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Messages that already exist are plain texts, they are wrapped into the JSON payload of a text message.
        migrationBuilder.Sql("""
            ALTER TABLE chat_messages
                ALTER COLUMN text TYPE jsonb
                USING jsonb_build_object('Type', 'Message', 'Text', text);
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE chat_messages
                ALTER COLUMN text TYPE character varying(4000)
                USING COALESCE(text ->> 'Text', '');
            """);
    }
}
