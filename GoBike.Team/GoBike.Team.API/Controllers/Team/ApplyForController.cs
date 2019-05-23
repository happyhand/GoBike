using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 車隊申請功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class ApplyForController : ControllerBase
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
        /// POST - 取消申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Cancel(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.CancelApplyForJoinTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("取消申請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Apply For Join Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("取消申請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 取得申請加入名單
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(TeamCommandDto teamCommand)
        {
            try
            {
                Tuple<IEnumerable<string>, string> result = await this.teamService.GetApplyForRequestList(teamCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Apply For Request List Error >>> TemaID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}\n{ex}");
                return BadRequest("取得申請加入名單發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Join(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.ApplyForJoinTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("申請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("申請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Reject(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.teamService.RejectApplyForJoinTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("拒絕申請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("拒絕申請加入車隊發生錯誤.");
            }
        }
    }
}