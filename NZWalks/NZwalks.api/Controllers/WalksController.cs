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

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;

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

        public async Task<IActionResult> UpdateWalkASync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
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

    }
}
