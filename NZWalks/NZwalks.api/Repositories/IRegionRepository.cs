using NZwalks.api.Models.Domain;

namespace NZwalks.api.Repositories
{
    public interface IRegionRepository
    {
       Task<IEnumerable<Region>> GetAllAsync();


    }
}
