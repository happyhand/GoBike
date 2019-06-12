using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Event
{
    /// <summary>
    /// 取得車隊活動列表
    /// </summary>
    [Route("api/TeamEvent/[controller]")]
    [ApiController]
    public class GetEventListOfTeamController : ControllerBase
    {
        /// <summary>
        /// eventService
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetEventListOfTeamController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="eventService">eventService</param>
        public GetEventListOfTeamController(ILogger<GetEventListOfTeamController> logger, IEventService eventService)
        {
            this.logger = logger;
            this.eventService = eventService;
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
                Tuple<IEnumerable<EventSimpleInfoDto>, string> result = await this.eventService.GetEventListOfTeam(teamCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event List Of Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("取得車隊活動列表發生錯誤.");
            }
        }
    }
}