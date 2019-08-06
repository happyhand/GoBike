using GoBike.Service.Repository.Models.Team;
using GoBike.Service.Service.Interface.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 搜尋車隊
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
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamDto teamDto)
        {
            try
            {
                Tuple<IEnumerable<TeamDto>, string> result = await this.teamService.SearchTeam(teamDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Team Error >>> ExecutorID:{teamDto.ExecutorID} SearchKey:{teamDto.SearchKey}\n{ex}");
                return BadRequest("搜尋車隊發生錯誤.");
            }
        }
    }
}