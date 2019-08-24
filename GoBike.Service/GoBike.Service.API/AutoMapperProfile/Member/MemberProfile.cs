using AutoMapper;
using GoBike.Service.Repository.Models.Member;
using GoBike.Service.Service.Models.Member;

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