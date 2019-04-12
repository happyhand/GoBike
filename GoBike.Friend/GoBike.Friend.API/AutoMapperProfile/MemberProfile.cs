using AutoMapper;
using GoBike.Friend.Repository.Models;
using GoBike.Friend.Service.Models;

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