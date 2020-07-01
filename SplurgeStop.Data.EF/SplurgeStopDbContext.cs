using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.Domain.LocationProfile;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Data.EF
{
    public sealed class SplurgeStopDbContext : DbContext
    {
        public string ConnectionString;
        public IConfigurationRoot Configuration { get; private set; }

        public SplurgeStopDbContext() { }

        public SplurgeStopDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DbSet<PurchaseTransaction> Purchases { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PurchaseTransactionConfig());
            modelBuilder.ApplyConfiguration(new LineItemConfig());
            modelBuilder.ApplyConfiguration(new StoreConfig());
            modelBuilder.ApplyConfiguration(new CityConfig());
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new LocationConfig());
            modelBuilder.ApplyConfiguration((new ProductConfig()));
            modelBuilder.ApplyConfiguration((new BrandConfig()));
            modelBuilder.ApplyConfiguration((new ProductTypeConfig()));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConnectionString is null)
            {
                ConnectionString = ConnectivityService.GetConnectionString();
            }

            optionsBuilder.UseSqlServer(ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
