using GoBike.Team.Repository.Interface;
using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Managers
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public class TeamService : ITeamService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<TeamService> logger;

        /// <summary>
        /// teamRepository
        /// </summary>
        private readonly ITeamRepository teamRepository;

        /// <summary>
        /// eventRepository
        /// </summary>
        private readonly IEventRepository eventRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamRepository">teamRepository</param>
        /// <param name="eventRepository">eventRepository</param>
        public TeamService(ILogger<TeamService> logger, ITeamRepository teamRepository, IEventRepository eventRepository)
        {
            this.logger = logger;
            this.teamRepository = teamRepository;
            this.eventRepository = eventRepository;
        }

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>string</returns>
        public async Task<Tuple<TeamInfoDto, string>> EditData(TeamInfoDto teamInfoDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        public async Task<Tuple<TeamInfoDto, string>> GetTeamInfo(TeamInfoDto teamInfoDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得車隊資訊
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        public async Task<Tuple<TeamInfoDto, string>> GetTeamInfoList(TeamInfoDto teamInfoDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得車隊資訊列表
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        public async Task<string> Register(TeamInfoDto teamInfoDto)
        {
            throw new NotImplementedException();
        }
    }
}