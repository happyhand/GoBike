using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 車隊邀請功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class InviteController : ControllerBase
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
        /// POST - 取消邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Cancel(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.CancelInviteJoinTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("取消邀請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Invite Join Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("取消邀請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 取得邀請加入名單
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(TeamCommandDto teamCommand)
        {
            try
            {
                Tuple<IEnumerable<TeamInfoDto>, string> result = await this.teamService.GetInviteRequestList(teamCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Invite Request List Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("取得邀請加入名單發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Join(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.InviteJoinTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("邀請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("邀請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 邀請多人加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> ManyJoin(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.InviteManyJoinTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("邀請多人加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Many Join Team Error >>> TemaID:{teamCommand.TeamID} TargetIDs:{JsonConvert.SerializeObject(teamCommand.TargetIDs)}\n{ex}");
                return BadRequest("邀請多人加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Reject(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.RejectInviteJoinTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("拒絕邀請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Invite Join Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("拒絕邀請加入車隊發生錯誤.");
            }
        }
    }
}