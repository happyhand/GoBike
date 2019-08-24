using AutoMapper;
using GoBike.Service.Repository.Models.Team;
using GoBike.Service.Service.Models.Team;

namespace GoBike.Service.API.AutoMapperProfile.Team
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamData, TeamDto>();
            CreateMap<TeamAnnouncementData, TeamAnnouncementDto>();
        }
    }
}