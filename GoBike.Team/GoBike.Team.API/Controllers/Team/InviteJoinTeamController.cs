using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 邀請加入車隊
    /// </summary>
    [Route("api/team/[controller]")]
    [ApiController]
    public class InviteJoinTeamController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<InviteJoinTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public InviteJoinTeamController(ILogger<InviteJoinTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.teamService.InviteJoinTeam(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("邀請加入車隊成功");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TemaID:{interactiveInfo.TeamID} MemberID:{interactiveInfo.MemberID}\n{ex}");
                return BadRequest("邀請加入車隊發生錯誤.");
            }
        }
    }
}