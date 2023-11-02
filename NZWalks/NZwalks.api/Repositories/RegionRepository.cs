using Microsoft.EntityFrameworkCore;
using NZwalks.api.Data;
using NZwalks.api.Models.Domain;

namespace NZwalks.api.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NzWalksDbContext nZWalksDbContext;
        public RegionRepository(NzWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync(); 
            //return  nZWalksDbContext.Regions;
        }
    }
}
