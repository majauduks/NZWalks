using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZwalks.api.Data;
using NZwalks.api.Models.DTO;
using NZwalks.api.Repositories;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;


namespace NZwalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {

        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {

            // fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();
            // convert 
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            // return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }


        [HttpPost]
        [ActionName("AddWalkAsync")]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {

            if (!(await ValidateAddWalksAsync(addWalkRequest)))
                    {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain Object 

            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };
            //  var walkDomain = mapper.Map<Walk> (addWalkRequest);

            // Pass domain object to Repository to persist this

            await walkRepository.AddAsync(walkDomain);

            // Convert the domain object back to DTO

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            walkDTO.Id = walkDomain.Id;

            //var walkDTO = new Models.DTO.Walk
            //{
            //    Id = walkDomain.Id,
            //    Length = walkDomain.Length,
            //    Name = walkDomain.Name,
            //    RegionId = walkDomain.RegionId,
            //    WalkDifficulty = walkDomain.WalkDifficulty,
            //};

            // Return the DTO to the client

            return CreatedAtAction(nameof(AddWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // validate the incoming request

            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            var updatedWalkDomain = new Models.Domain.Walk
            {

                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };

            updatedWalkDomain = await walkRepository.UpdateAsync(id, updatedWalkDomain);

            if (updatedWalkDomain == null)
            {
                return NotFound();
            }
            else
            {
                // var updatedWalkDTO = mapper.Map<Models.DTO.Walk>(updatedWalkDomain);

                var updatedWalkDTO = new Models.DTO.Walk
                {
                    Id = updatedWalkDomain.Id,
                    Length = updatedWalkDomain.Length,
                    Name = updatedWalkDomain.Name,
                    RegionId = updatedWalkDomain.RegionId,
                    WalkDifficultyId = updatedWalkDomain.WalkDifficultyId,
                    
                };

            return Ok(updatedWalkDTO);
            }


        }


        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);

            if(walkDomain == null)
            {
                return NotFound();
            }

           var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }

        #region Private methods
        
        private async Task<bool> ValidateAddWalksAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), "Data is required");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
                $"{nameof(addWalkRequest.Name)} is required");
            }

            if(addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
               $"{nameof(addWalkRequest.Length)} should be greater than zero");
            }

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
               $"{nameof(addWalkRequest.RegionId)} is invalid");
            }
            
            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if(walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
               $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), "Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                $"{nameof(updateWalkRequest.Name)} is required");
            }

            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
               $"{nameof(updateWalkRequest.Length)} should be greater than zero");
            }

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
               $"{nameof(updateWalkRequest.RegionId)} is invalid");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
               $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");
            }

            if (ModelState.ErrorCount > 0)
            { 
                return false;
            }

           return true;

        }

        #endregion

    }
}
