using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 更新車隊隊長
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class UpdateTeamLeaderController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<UpdateTeamLeaderController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public UpdateTeamLeaderController(ILogger<UpdateTeamLeaderController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.UpdateTeamLeader(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("更新車隊隊長成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Leader Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("更新車隊隊長發生錯誤.");
            }
        }
    }
}