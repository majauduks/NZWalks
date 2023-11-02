using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZwalks.api.Models.Domain;
using NZwalks.api.Repositories;

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
        public async Task<IActionResult> GetAllRegions()
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
    }
}
