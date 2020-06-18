using System.Threading.Tasks;

namespace SplurgeStop.Domain.CityProfile
{
    public interface ICityService
    {
        Task Handle(object command);
    }
}
