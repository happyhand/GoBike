using AutoMapper;
using GoBike.Interactive.Repository.Models;
using GoBike.Interactive.Service.Models;

namespace GoBike.Interactive.API.AutoMapperProfile
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberData, MemberInfoDto>().ReverseMap();
        }
    }
}