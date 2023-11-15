using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

            if (region == null)
            {
                return NotFound();
            }

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Validate the request

            //if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

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

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // get region from database
            var region = await regionRepository.DeleteAsync(id);

            // if null NotFound

            if (region == null)
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
            //if(!ValidateUpdateRegionAsync(updateReqionRequest))
            //{
            //    return BadRequest(ModelState);
            //}
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

            if (region == null)
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

            return Ok(regionDTO);

        }


        #region Private methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {

            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), "Data is required");
                return false;

            }
                if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
                {
                    ModelState.AddModelError(nameof(addRegionRequest.Code), "The code cannot be empty or null or whitespace");
                }

                if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
                {
                    ModelState.AddModelError(nameof(addRegionRequest.Name), "The name cannot be empty or null or whitespace");

                }

                if (addRegionRequest.Area <= 0)
                {
                    ModelState.AddModelError(nameof(addRegionRequest.Area), "Area cannot be less than or equal to zero");

                }

                //if (addRegionRequest.Lat <= 0)
                //{
                //    ModelState.AddModelError(nameof(addRegionRequest.Lat), "Latitude cannot be less than or equal to zero");

                //}

                //if (addRegionRequest.Long <= 0)
                //{
                //    ModelState.AddModelError(nameof(addRegionRequest.Long), "Longitute cannot be less than or equal to zero");

                //}

                if (addRegionRequest.Population < 0)
                {
                    ModelState.AddModelError(nameof(addRegionRequest.Population), "Popoulation cannot be less than zero");


                }

                if (ModelState.ErrorCount > 0)
                {
                    return false;
                }

                return true;

            
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {

            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), "Data is required");
                return false;

            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), "The code cannot be empty or null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), "The name cannot be empty or null or whitespace");

            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), "Area cannot be less than or equal to zero");

              }

                //if (updateRegionRequest.Lat <= 0)
                //{
                //    ModelState.AddModelError(nameof(updateRegionRequest.Lat), "Latitude cannot be less than or equal to zero");

                //}

            //    if (updateRegionRequest.Long <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateRegionRequest.Long), "Longitute cannot be less than or equal to zero");

            //}

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), "Popoulation cannot be less than zero");


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