// Created on 31/08/2026 22:15 by Laserson

using Lidemia.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lidemia.DataAccess.Context;

public partial class LidemiaDbContext : DbContext
{
    public LidemiaDbContext(DbContextOptions<LidemiaDbContext> options) : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

    public DbSet<SupplierEntity> Suppliers => Set<SupplierEntity>();

    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    public DbSet<ProductPhotoEntity> ProductsPhotos => Set<ProductPhotoEntity>();

    public DbSet<PhotoEntity> Photos => Set<PhotoEntity>();

    public DbSet<PromoCodeEntity> PromoCodes => Set<PromoCodeEntity>();

    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();

    public DbSet<WishListEntity> WishLists => Set<WishListEntity>();

    public DbSet<WishListItemEntity> WishListItems => Set<WishListItemEntity>();

    public DbSet<ShoppingCartEntity> ShoppingCarts => Set<ShoppingCartEntity>();

    public DbSet<ProductFeedbackEntity> ProductFeedbacks => Set<ProductFeedbackEntity>();

    public DbSet<AuthTokenEntity> AuthTokens => Set<AuthTokenEntity>();

    public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();

    public DbSet<SupplierStatsEntity> SuppliersStats => Set<SupplierStatsEntity>();

    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();

    public DbSet<CustomerAddressEntity> CustomerAddresses => Set<CustomerAddressEntity>();

    public DbSet<SupplierAddressEntity> SuppliersAddresses => Set<SupplierAddressEntity>();

    public DbSet<PasswordResetEntity> PasswordResets => Set<PasswordResetEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}