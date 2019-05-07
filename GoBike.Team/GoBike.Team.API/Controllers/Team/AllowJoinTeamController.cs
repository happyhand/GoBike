using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 允許加入車隊
    /// </summary>
    [Route("api/team/[controller]")]
    [ApiController]
    public class AllowJoinTeamController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AllowJoinTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public AllowJoinTeamController(ILogger<AllowJoinTeamController> logger, ITeamService teamService)
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
                string result = await this.teamService.JoinTeam(teamCommand, true);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("允許加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Allow Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("允許加入車隊發生錯誤.");
            }
        }
    }
}