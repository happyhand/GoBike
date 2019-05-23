using AutoMapper;
using GoBike.API.Service.Models.Team.Command.Data;
using GoBike.API.Service.Models.Team.View;

namespace GoBike.API.App.AutoMapperProfile.Team
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamInfoDto, TeamDetailInfoViewDto>();
        }
    }
}