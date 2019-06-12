using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Event
{
    /// <summary>
    /// 刪除活動
    /// </summary>
    [Route("api/TeamEvent/[controller]")]
    [ApiController]
    public class DeleteEventController : ControllerBase
    {
        /// <summary>
        /// eventService
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<DeleteEventController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="eventService">eventService</param>
        public DeleteEventController(ILogger<DeleteEventController> logger, IEventService eventService)
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
                string result = await this.eventService.DeleteEvent(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除活動成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}\n{ex}");
                return BadRequest("刪除活動發生錯誤.");
            }
        }
    }
}