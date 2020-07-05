using System.Threading.Tasks;

namespace SplurgeStop.Domain.Shared
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
