using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;

namespace NZwalks.api.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();
               
        }
    }
}
