using AutoMapper;
using GoBike.API.App.Models.Member;
using GoBike.API.Service.Models.Member.Data;

namespace GoBike.API.App.AutoMapperProfile.Member
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberInfoDto, MemberSimpleViewDto>().ReverseMap();
            CreateMap<MemberInfoDto, MemberDetailViewDto>().ReverseMap();
        }
    }
}