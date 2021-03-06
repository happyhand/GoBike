﻿using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 車隊編輯
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class EditDataController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditDataController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public EditDataController(ILogger<EditDataController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamCommandDto teamCommand)
        {
            try
            {
                Tuple<TeamInfoDto, string> result = await this.teamService.EditData(teamCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(teamCommand)}\n{ex}");
                return BadRequest("車隊編輯發生錯誤.");
            }
        }
    }
}