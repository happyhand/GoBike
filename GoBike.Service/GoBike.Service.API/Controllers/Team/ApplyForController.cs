using GoBike.Service.Service.Interface.Team;
using GoBike.Service.Service.Models.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 申請加入
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
        /// 允許申請加入
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AllowJoin(TeamDto teamDto)
        {
            try
            {
                string result = await this.teamService.JoinTeam(teamDto, false);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Allow Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return BadRequest("加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 拒絕申請加入
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> RejectJoin(TeamDto teamDto)
        {
            try
            {
                string result = await this.teamService.RejectApplyForJoinTeam(teamDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("拒絕申請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return BadRequest("拒絕申請加入車隊發生錯誤.");
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
                string result = await this.teamService.ApplyForJoinTeam(teamDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("申請加入車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return BadRequest("申請加入車隊發生錯誤.");
            }
        }
    }
}