using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZwalks.api.Data;
using NZwalks.api.Models.DTO;
using NZwalks.api.Repositories;

namespace NZwalks.api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalkDifficultiesController : Controller
    {
        
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }


        [HttpGet]

        public async Task<IActionResult> GetAllWalkDifficultiesAsync ()
        {
            // fetch from database (domain walks)
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();
             


            // convert

            var walkDifficultiesDTO = mapper.Map<List<Models.Domain.WalkDifficulty>>(walkDifficultiesDomain);

            // return response

            return Ok(walkDifficultiesDTO);
        }


        [HttpGet("{id:guid}")]

        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetAsync(id);

            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.Domain.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {

            var updatedWalkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
            };

            updatedWalkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, updatedWalkDifficultyDomain);

            if(updatedWalkDifficultyDomain == null)
            {
                return NotFound();
            }

            var updatedWalkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(updatedWalkDifficultyDomain);
            return Ok(updatedWalkDifficultyDTO);

         }


        [HttpPost]
        [ActionName("AddWalkDifficultyAsync")]

        public async Task<IActionResult> AddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // request (DTO) to domain object
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };
            // pass details to the repository
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            // convert the data back to a DTO

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return CreatedAtAction(nameof(AddWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            if(walkDifficulty == null)
            
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);

        }



    }
}
