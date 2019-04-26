﻿using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 強制離開車隊
    /// </summary>
    [Route("api/team/[controller]")]
    [ApiController]
    public class ForceLeaveTeamController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ForceLeaveTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public ForceLeaveTeamController(ILogger<ForceLeaveTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamAction">teamAction</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamActionDto teamAction)
        {
            try
            {
                string result = await this.teamService.ForceLeaveTeam(teamAction);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("強制離開車隊成功");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Force Leave Team Error >>> TeamID:{teamAction.TeamID} MemberID:{teamAction.MemberID} ActionID:{teamAction.ActionID}\n{ex}");
                return BadRequest("強制離開車隊發生錯誤.");
            }
        }
    }
}