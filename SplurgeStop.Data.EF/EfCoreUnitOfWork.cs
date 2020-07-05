using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Data.EF
{
    public sealed class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly SplurgeStopDbContext _dbContext;

        public EfCoreUnitOfWork(SplurgeStopDbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task Commit()
            => await _dbContext.SaveChangesAsync();
    }
}
