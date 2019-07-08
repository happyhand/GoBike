using AutoMapper;
using GoBike.Service.Repository.Models.Member;

namespace GoBike.Service.API.AutoMapperProfile.Member
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberData, MemberDto>();
            CreateMap<RideData, RideDto>().ReverseMap();
        }
    }
}