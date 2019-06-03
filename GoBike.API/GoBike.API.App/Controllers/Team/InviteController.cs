using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 車隊邀請功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class InviteController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<InviteController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public InviteController(ILogger<InviteController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 車隊邀請功能 - 取消邀請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Cancel(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.CancelInviteJoinTeam(memberID, teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Invite Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} MemberID:{teamInteractiveCommand.MemberID}\n{ex}");
                return BadRequest("取消邀請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊邀請功能 - 取得邀請加入名單
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Get()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.GetInviteRequestList(memberID);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Invite Request List Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("取得邀請加入名單發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊邀請功能 - 邀請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Join(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.InviteJoinTeam(memberID, teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} MemberID:{teamInteractiveCommand.MemberID}\n{ex}");
                return BadRequest("邀請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊邀請功能 - 邀請多人加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> ManyJoin(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.InviteManyJoinTeam(memberID, teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Many Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} MemberList:{JsonConvert.SerializeObject(teamInteractiveCommand.MemberList)}\n{ex}");
                return BadRequest("邀請多人加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊邀請功能 - 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Reject(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            teamInteractiveCommand.MemberID = memberID;
            try
            {
                ResponseResultDto responseResult = await this.teamService.RejectInviteJoinTeam(teamInteractiveCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} TargetID:{memberID}\n{ex}");
                return BadRequest("拒絕邀請加入車隊發生錯誤.");
            }
        }
    }
}