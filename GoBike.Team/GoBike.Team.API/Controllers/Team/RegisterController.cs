﻿using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 建立車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<RegisterController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public RegisterController(ILogger<RegisterController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamInfoDto teamInfo)
        {
            try
            {
                string result = await this.teamService.Register(teamInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("建立車隊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Data:{JsonConvert.SerializeObject(teamInfo)}\n{ex}");
                return BadRequest("建立車隊發生錯誤.");
            }
        }
    }
}