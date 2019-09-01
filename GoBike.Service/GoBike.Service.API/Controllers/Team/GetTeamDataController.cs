using GoBike.Service.Service.Interface.Team;
using GoBike.Service.Service.Models.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 取得車隊資料
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class GetTeamDataController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetTeamDataController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetTeamDataController(ILogger<GetTeamDataController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamDto teamDto)
        {
            try
            {
                Tuple<TeamDto, string> result = await this.teamService.GetTeamData(teamDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return BadRequest("取得車隊資料發生錯誤.");
            }
        }
    }
}