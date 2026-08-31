using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> AsJsonb<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
    {
        return propertyBuilder.HasColumnType("jsonb");
    }
}