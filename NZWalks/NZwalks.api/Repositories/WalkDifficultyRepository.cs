using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZwalks.api.Data;
using NZwalks.api.Models.Domain;

namespace NZwalks.api.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NzWalksDbContext nZWalksDbContext;
        private readonly IMapper mapper;

        public WalkDifficultyRepository(NzWalksDbContext nZWalksDbContext, IMapper mapper)
        {
            this.nZWalksDbContext = nZWalksDbContext;
            this.mapper = mapper;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulties.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<Models.Domain.WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.WalkDifficulties.FindAsync(id);
            if (existingWalk != null)
            {
            nZWalksDbContext.WalkDifficulties.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;

            }

            return null;
        }



        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            var walkDifficultiesList = await nZWalksDbContext.WalkDifficulties.ToListAsync();
            return walkDifficultiesList;
            
        }

        public async Task<NZwalks.api.Models.Domain.WalkDifficulty> GetAsync(Guid id)
        {
            var walkdDifficulty = await nZWalksDbContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
           
            return walkdDifficulty;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalkDifficulties.FindAsync(id);
            if(existingWalkDifficulty != null)
            {
                existingWalkDifficulty.Code = walkDifficulty.Code;
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalkDifficulty;
            }

            return null;
        }
    }
}
