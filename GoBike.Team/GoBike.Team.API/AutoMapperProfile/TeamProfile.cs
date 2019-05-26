using AutoMapper;
using GoBike.Team.Repository.Models;
using GoBike.Team.Service.Models.Data;

namespace GoBike.Team.API.AutoMapperProfile
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamData, TeamInfoDto>().ReverseMap();
            CreateMap<AnnouncementData, AnnouncementInfoDto>().ReverseMap();
        }
    }
}