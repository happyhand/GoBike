using AutoMapper;
using GoBike.Service.Repository.Models.Common;
using GoBike.Service.Service.Models.Common;

namespace GoBike.Service.API.AutoMapperProfile.Common
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<CityData, CityDto>();
        }
    }
}