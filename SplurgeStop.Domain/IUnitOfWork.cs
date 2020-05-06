using System.Threading.Tasks;

namespace SplurgeStop.Domain
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
