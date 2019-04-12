using AutoMapper;
using GoBike.Member.Repository.Models;
using GoBike.Member.Service.Models;

namespace GoBike.Member.API.AutoMapperProfile
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberData, MemberInfoDto>().ReverseMap();
        }
    }
}