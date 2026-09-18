// Created on 11/10/2021 23:24 by Andrey Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lidemia.DataAccess.Extensions;

public static class EntityTypeBuilderExtensions
{
    private static void AddInitialColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IDbEntity
    {
        builder.Property(e => e.RowVersion).HasDefaultValue(1);

        builder.Property(e => e.IsActive).IsRequired();

        builder.Property(e => e.CreateDate).HasDefaultValueSql("now()");
        builder.Property(e => e.UpdateDate);

        builder.HasIndex(x => x.IsActive);
    }

    public static void AddBaseColumns<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
        where TKey : notnull
        where TEntity : class, IDbEntity, IHasId<TKey>
    {
        builder.HasKey(x => x.Id);

        if (typeof(TKey) == typeof(string))
        {
            builder.Property(e => e.Id);
        }
        else
        {
            builder.Property(e => e.Id).AsServiceId();
        }

        builder.AddInitialColumns();
    }

    public static void AddBaseColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IDbEntity
    {
        builder.HasNoKey();
        builder.AddInitialColumns();
    }

    public static void AddBaseColumnsMeta<TEntity, TKey, TMetadata>(this EntityTypeBuilder<TEntity> entity)
        where TKey : notnull
        where TEntity : class, IDbEntity, IHasId<TKey>, IHasMetadata<TMetadata>
    {
        entity.AddBaseColumns<TEntity, TKey>();
        entity.Property(x => x.Metadata).AsJsonb();
    }
}