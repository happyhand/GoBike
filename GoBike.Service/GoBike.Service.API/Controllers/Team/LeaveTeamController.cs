using GoBike.Service.Service.Interface.Team;
using GoBike.Service.Service.Models.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 離開車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class LeaveTeamController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<LeaveTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public LeaveTeamController(ILogger<LeaveTeamController> logger, ITeamService teamService)
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
                string result = await this.teamService.LeaveTeam(teamDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("離開車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return BadRequest("離開車隊發生錯誤.");
            }
        }
    }
}