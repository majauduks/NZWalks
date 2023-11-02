﻿using NZwalks.api.Models.Domain;

namespace NZwalks.api.Repositories
{
    public interface IRegionRepository
    {
       Task<IEnumerable<Region>> GetAllAsync ();

        Task<Region> GetAsync(Guid id);

        Task<Region> AddASync(Region region);

        Task<Region> DeleteAsync(Guid id);

        Task<Region> UpdateAsync(Guid id, Region region);


    }
}
