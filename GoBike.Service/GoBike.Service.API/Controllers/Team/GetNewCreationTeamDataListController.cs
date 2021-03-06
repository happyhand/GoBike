﻿using GoBike.Service.Service.Interface.Team;
using GoBike.Service.Service.Models.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 取得新創車隊資料列表
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class GetNewCreationTeamDataListController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetNewCreationTeamDataListController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetNewCreationTeamDataListController(ILogger<GetNewCreationTeamDataListController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Tuple<IEnumerable<TeamDto>, string> result = await this.teamService.GetNewCreationTeamDataList();
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get New Creation Team Data List Error\n{ex}");
                return BadRequest("取得新創車隊資料列表發生錯誤.");
            }
        }
    }
}