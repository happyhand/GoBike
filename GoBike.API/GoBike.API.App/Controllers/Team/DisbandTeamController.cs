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
    /// 解散車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class DisbandTeamController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<DisbandTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public DisbandTeamController(ILogger<DisbandTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 解散車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(TeamCommandDto teamCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            teamCommand.ExaminerID = memberID;
            try
            {
                ResponseResultDto responseResult = await this.teamService.DisbandTeam(teamCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Disband Team Error >>> TeamID:{teamCommand.TeamID} MemberID:{memberID}\n{ex}");
                return BadRequest("解散車隊發生錯誤.");
            }
        }
    }
}