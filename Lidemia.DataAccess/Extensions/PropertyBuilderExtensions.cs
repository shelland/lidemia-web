using Lidemia.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> AsJsonb<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
    {
        return propertyBuilder.HasColumnType("jsonb");
    }

    public static PropertyBuilder<TProperty> AsGeography<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
    {
        return propertyBuilder.HasColumnType($"geography(point, {Constants.DefaultSrid})");
    }

    public static PropertyBuilder<TProperty> AsServiceId<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
    {
        return propertyBuilder.HasDefaultValueSql("service.next_id()").ValueGeneratedOnAdd();
    }

    public static PropertyBuilder<TProperty> AsGuidV7<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
    {
        return propertyBuilder.HasDefaultValueSql("uuidv7()").ValueGeneratedNever();
    }
}