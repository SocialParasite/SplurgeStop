using System;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Data.EF;
using Xunit;

namespace SplurgeStop.Integration.Tests
{
    public sealed class DatabaseFixture : IDisposable
    {
        internal SplurgeStopDbContext context;
        internal string connectionString;

        public DatabaseFixture()
        {
            connectionString = ConnectivityService.GetConnectionString("TEMP");
            context = new SplurgeStopDbContext(connectionString);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            context.Database.ExecuteSqlRaw("DELETE FROM LineItem");
            context.Database.ExecuteSqlRaw("DELETE FROM Products");
            context.Database.ExecuteSqlRaw("DELETE FROM Brands");
            context.Database.ExecuteSqlRaw("DELETE FROM ProductTypes");
            context.Database.ExecuteSqlRaw("DELETE FROM Size");

            context.Database.ExecuteSqlRaw("DELETE FROM Purchases");
            context.Database.ExecuteSqlRaw("DELETE FROM Stores");
            context.Database.ExecuteSqlRaw("DELETE FROM Locations");
            context.Database.ExecuteSqlRaw("DELETE FROM Cities");
            context.Database.ExecuteSqlRaw("DELETE FROM Countries");
        }
    }

    [Trait("Integration", "Database")]
    public sealed partial class DatabaseTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;

        public DatabaseTests(DatabaseFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }
    }
}
