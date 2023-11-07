using NZwalks.api.Models.Domain;

namespace NZwalks.api.Repositories

{
    public interface IWalkDifficultyRepository
    {
        Task<IEnumerable<NZwalks.api.Models.Domain.WalkDifficulty>> GetAllAsync();

        Task<NZwalks.api.Models.Domain.WalkDifficulty> GetAsync(Guid id);

        Task<NZwalks.api.Models.Domain.WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);

        Task<NZwalks.api.Models.Domain.WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);

        Task<NZwalks.api.Models.Domain.WalkDifficulty> DeleteAsync(Guid id);

         

























    }
}
