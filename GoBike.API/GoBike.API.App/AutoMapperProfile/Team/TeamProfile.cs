using AutoMapper;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Team.Data;
using GoBike.API.Service.Models.Team.View;

namespace GoBike.API.App.AutoMapperProfile.Team
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamDto, TeamSimpleInfoView>();
            CreateMap<TeamDto, TeamNoJoinInfoView>();
            CreateMap<MemberDto, TeamMemberInfoView>();
        }
    }
}