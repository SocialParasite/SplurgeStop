using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Data.EF
{
    public sealed class SplurgeStopDbContext : DbContext
    {
        private string _connectionString;
        public IConfigurationRoot Configuration { get; private set; }

        public SplurgeStopDbContext() { }

        public SplurgeStopDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<PurchaseTransaction> Purchases { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Size> Size { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BrandConfig).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _connectionString ??= ConnectivityService.GetConnectionString();

            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
