using AutoMapper;
using GoBike.Member.Repository.Models;
using GoBike.Member.Service.Models;

namespace GoBike.Member.API.AutoMapperProfile
{
    public class BikeProfile : Profile
    {
        public BikeProfile()
        {
            CreateMap<BikeData, BikeInfoDto>().ReverseMap();
        }
    }
}