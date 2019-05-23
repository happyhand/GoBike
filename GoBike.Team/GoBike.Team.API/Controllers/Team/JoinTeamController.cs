using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 加入車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class JoinTeamController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<JoinTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public JoinTeamController(ILogger<JoinTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST - 允許加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AllowJoin(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.JoinTeam(teamCommand, false);
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

        /// <summary>
        /// POST - 邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> InviteJoin(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.JoinTeam(teamCommand, true);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("邀請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("邀請加入車隊發生錯誤.");
            }
        }
    }
}