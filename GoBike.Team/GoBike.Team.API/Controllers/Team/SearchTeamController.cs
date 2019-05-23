using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 搜尋車隊資訊列表
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class SearchTeamController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SearchTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public SearchTeamController(ILogger<SearchTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamSearchCommandDto teamSearchCommand)
        {
            try
            {
                Tuple<IEnumerable<TeamInfoDto>, string> result = await this.teamService.SearchTeam(teamSearchCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Team Error >>> SearcherID:{teamSearchCommand.SearcherID} SearchKey:{teamSearchCommand.SearchKey}\n{ex}");
                return BadRequest("搜尋車隊資訊列表發生錯誤.");
            }
        }
    }
}