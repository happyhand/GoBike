using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 更新車隊副隊長
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class UpdateTeamViceLeaderController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<UpdateTeamViceLeaderController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public UpdateTeamViceLeaderController(ILogger<UpdateTeamViceLeaderController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST - 新增車隊副隊長
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Add(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.interactiveService.UpdateTeamViceLeader(teamCommand, true);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("新增車隊副隊長成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Team Vice Leader Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("新增車隊副隊長發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 移除車隊副隊長
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Remove(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.interactiveService.UpdateTeamViceLeader(teamCommand, false);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("移除車隊副隊長成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Team Vice Leader Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("移除車隊副隊長發生錯誤.");
            }
        }
    }
}