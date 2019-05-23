using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 離開車隊
    /// </summary>
    [ApiController]
    public class LeaveTeamController : ApiController
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
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Team/[controller]")]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            teamInteractiveCommand.MemberID = memberID;
            try
            {
                ResponseResultDto responseResult = await this.teamService.LeaveTeam(teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TeamID:{teamInteractiveCommand.TeamID} MemberID:{memberID} \n{ex}");
                return BadRequest("離開車隊發生錯誤.");
            }
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Team/[controller]/Force")]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Force(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.ForceLeaveTeam(memberID, teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TeamID:{teamInteractiveCommand.TeamID} MemberID:{memberID} \n{ex}");
                return BadRequest("離開車隊發生錯誤.");
            }
        }
    }
}