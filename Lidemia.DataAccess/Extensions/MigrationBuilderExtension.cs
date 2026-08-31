// Created on 13/11/2023 22:38 by shell

using System.Reflection;
using Lidemia.Core.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lidemia.DataAccess.Extensions;

public static class MigrationBuilderExtension
{
    public static void PreInit(this MigrationBuilder builder)
    {
        builder.Sql(FileProviderHelper.ReadEmbedded(Assembly.GetExecutingAssembly(), "Scripts\\Initial.sql"));
    }

    public static void PostInit(this MigrationBuilder builder)
    {
        builder.Sql(FileProviderHelper.ReadEmbedded(Assembly.GetExecutingAssembly(), "Scripts\\Seed.sql"));
    }
}