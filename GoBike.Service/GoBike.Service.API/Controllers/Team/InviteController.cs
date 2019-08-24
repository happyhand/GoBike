using GoBike.Service.Service.Interface.Team;
using GoBike.Service.Service.Models.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 邀請加入
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
        /// 同意邀請加入
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AgreeInvite(TeamDto teamDto)
        {
            try
            {
                string result = await this.teamService.AgreeInviteJoinTeam(teamDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("同意邀請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Agree Invite Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return BadRequest("同意邀請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 允許邀請加入
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AllowJoin(TeamDto teamDto)
        {
            try
            {
                string result = await this.teamService.JoinTeam(teamDto, true);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Allow Invite Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return BadRequest("加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 拒絕邀請加入
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> RejectJoin(TeamDto teamDto)
        {
            try
            {
                string result = await this.teamService.RejectInviteJoinTeam(teamDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("拒絕邀請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Invite For Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return BadRequest("拒絕邀請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 請求加入
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> RequestJoin(TeamDto teamDto)
        {
            try
            {
                string result = await this.teamService.InviteManyJoinTeam(teamDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("邀請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID} TargetIDs:{JsonConvert.SerializeObject(teamDto.TargetIDs)}\n{ex}");
                return BadRequest("邀請加入車隊發生錯誤.");
            }
        }
    }
}