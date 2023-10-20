using KoreanSecrets.Domain.Configuration;
using KoreanSecrets.Domain.Entities;
using KoreanSecrets.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.DbConnection;

public class DataContext : IdentityDbContext<User, ApplicationRole, Guid>
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }


    public DbSet<AddressInfo> Addresses { get; set; }
    public DbSet<AppFile> Files { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Bucket> Buckets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Demand> Demands { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Promocode> Promocodes { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchasedProduct> PurchasedProducts { get; set; }
    public DbSet<CategoryBrand> CategoryBrands { get; set; }
    public DbSet<CategoryCountry> CategoryCountries { get; set; }
    public DbSet<CategoryDemand> CategoryDemands { get; set; }
    public DbSet<CategorySubCategory> CategorySubCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new BannerConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new BrandConfiguration());
        builder.ApplyConfiguration(new DemandConfiguration());
        builder.ApplyConfiguration(new CountryConfiguration());
        builder.ApplyConfiguration(new SubCategoryConfiguration());
        builder.ApplyConfiguration(new PurchaseConfiguration());
        builder.ApplyConfiguration(new PurchaseProductConfiguration());
        builder.ApplyConfiguration(new CategoryBrandConfiguration());
        builder.ApplyConfiguration(new CategorySubCategoryConfiguration());
        builder.ApplyConfiguration(new CategoryCountryConfiguration());
        builder.ApplyConfiguration(new CategoryDemandConfiguration());
    }
}
