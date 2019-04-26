﻿using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 離開車隊
    /// </summary>
    [Route("api/team/[controller]")]
    [ApiController]
    public class LeaveTeamController : ControllerBase
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
        /// <param name="teamAction">teamAction</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamActionDto teamAction)
        {
            try
            {
                string result = await this.teamService.LeaveTeam(teamAction);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("離開車隊成功");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TemaID:{teamAction.TeamID} MemberID:{teamAction.MemberID}\n{ex}");
                return BadRequest("離開車隊發生錯誤.");
            }
        }
    }
}