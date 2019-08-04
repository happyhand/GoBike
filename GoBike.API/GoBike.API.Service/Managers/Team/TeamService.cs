using AutoMapper;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Managers.Team
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
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public TeamService(ILogger<TeamService> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> CreateTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/CreateTeam", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "建立車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 編輯車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> EditTeamData(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/EditTeamData", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Data Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "編輯車隊資料發生錯誤."
                };
            }
        }
    }
}