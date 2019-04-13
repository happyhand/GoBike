using AutoMapper;
using GoBike.API.App.Models.Member;
using GoBike.API.Service.Models.Member;

namespace GoBike.API.App.AutoMapperProfile.Member
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberInfoDto, MemberViewDto>().ReverseMap();
            CreateMap<MemberInfoDto, MemberInteractiveViewDto>().ReverseMap();
        }
    }
}