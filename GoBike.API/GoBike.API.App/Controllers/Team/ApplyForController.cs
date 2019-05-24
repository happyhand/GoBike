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
    /// 車隊申請功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class ApplyForController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ApplyForController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public ApplyForController(ILogger<ApplyForController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 車隊申請功能 - 取消申請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Cancel(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            teamInteractiveCommand.MemberID = memberID;
            try
            {
                ResponseResultDto responseResult = await this.teamService.CancelApplyForJoinTeam(teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Apply For Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} MemberID:{memberID}\n{ex}");
                return BadRequest("申請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊申請功能 - 申請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Join(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            teamInteractiveCommand.MemberID = memberID;
            try
            {
                ResponseResultDto responseResult = await this.teamService.ApplyForJoinTeam(teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} MemberID:{memberID}\n{ex}");
                return BadRequest("申請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊申請功能 - 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Reject(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.RejectApplyForJoinTeam(memberID, teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{memberID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return BadRequest("拒絕申請加入車隊發生錯誤.");
            }
        }
    }
}