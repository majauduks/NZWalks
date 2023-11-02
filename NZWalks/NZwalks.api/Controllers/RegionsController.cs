using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZwalks.api.Models.Domain;
using NZwalks.api.Models.DTO;
using NZwalks.api.Repositories;
using System.Runtime.InteropServices;

namespace NZwalks.api.Controllers
{
        [ApiController]
        [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper) 
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            // return DTO regions
            //    var regionsDTO = new List<Models.DTO.Region>();
            //    regions.ToList().ForEach(region =>
            //    {
            //        var regionDTO = new Models.DTO.Region()
            //        {
            //            Id = region.Id,
            //            Name = region.Name,
            //            Code = region.Code,
            //            Area = region.Area,
            //            Lat = region.Lat,
            //            Long = region.Long,
            //            Population = region.Population,
            //        };

            //        regionsDTO.Add(regionDTO);

            //    }); 
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {

            var region = await regionRepository.GetAsync(id);
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            
            if(region == null)
            {
                return NotFound();
            }

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
           
            // Request(DTO) to domain model

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };
            // pass details to the repository

             region = await regionRepository.AddASync(region);

            // convert the data back to  DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area, 
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new {id=regionDTO.Id}, regionDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // get region from database
            var region = await regionRepository.DeleteAsync(id);
            
            // if null NotFound

            if(region == null)
            {
                return NotFound();
            }

            // Convert response back to DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population

            };

            // return Ok response

            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateReqionRequest)
        {
            // Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = updateReqionRequest.Code,
                Area = updateReqionRequest.Area,
                Lat = updateReqionRequest.Lat,
                Long = updateReqionRequest.Long,
                Name = updateReqionRequest.Name,
                Population = updateReqionRequest.Population
            };

            // Update Region using repository

           region = await regionRepository.UpdateAsync(id, region);

            // if null NotFound

            if(region== null)
            {
                return NotFound();
            }


            // Convert Domain back to DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };


            // Return Ok reponse

            return Ok(regionDTO );

        }


    }
}