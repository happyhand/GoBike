using AutoMapper;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Team.Data;
using GoBike.API.Service.Models.Team.View;

namespace GoBike.API.App.AutoMapperProfile.Team
{
    /// <summary>
    /// TeamProfile
    /// </summary>
    public class TeamProfile : Profile
    {
        /// <summary>
        /// TeamProfile
        /// </summary>
        public TeamProfile()
        {
            CreateMap<TeamDto, TeamSimpleInfoViewDto>();
            CreateMap<TeamDto, TeamDetailInfoViewDto>();
            CreateMap<TeamDto, TeamNoJoinInfoViewDto>();
            CreateMap<MemberDto, TeamMemberInfoViewDto>();
            CreateMap<TeamEventDto, TeamEventSimpleInfoViewDto>();
            CreateMap<TeamEventDto, TeamEventDetailInfoViewDto>();
        }
    }
}