using AutoMapper;
using GoBike.Team.Repository.Models;
using GoBike.Team.Service.Models;

namespace GoBike.Team.API.AutoMapperProfile
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<TeamData, TeamInfoDto>().ReverseMap();
        }
    }
}