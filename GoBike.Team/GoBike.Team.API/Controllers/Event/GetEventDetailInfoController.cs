using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Event
{
    /// <summary>
    /// 取得活動詳細資訊
    /// </summary>
    [Route("api/TeamEvent/[controller]")]
    [ApiController]
    public class GetEventDetailInfoController : ControllerBase
    {
        /// <summary>
        /// eventService
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetEventDetailInfoController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="eventService">eventService</param>
        public GetEventDetailInfoController(ILogger<GetEventDetailInfoController> logger, IEventService eventService)
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
                Tuple<EventDetailInfoDto, string> result = await this.eventService.GetEventDetailInfo(teamCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event Detail Info Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}\n{ex}");
                return BadRequest("取得活動詳細資訊發生錯誤.");
            }
        }
    }
}