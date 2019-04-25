using AutoMapper;
using GoBike.Team.Repository.Models;
using GoBike.Team.Service.Models;

namespace GoBike.Team.API.AutoMapperProfile
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberData, MemberInfoDto>().ReverseMap();
        }
    }
}