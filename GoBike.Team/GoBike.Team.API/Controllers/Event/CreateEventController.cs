using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Event
{
    /// <summary>
    /// 建立活動
    /// </summary>
    [Route("api/TeamEvent/[controller]")]
    [ApiController]
    public class CreateEventController : ControllerBase
    {
        /// <summary>
        /// eventService
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<CreateEventController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="eventService">eventService</param>
        public CreateEventController(ILogger<CreateEventController> logger, IEventService eventService)
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
                string result = await this.eventService.CreateEvent(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("建立活動成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventInfo:{JsonConvert.SerializeObject(teamCommand.EventInfo)}\n{ex}");
                return BadRequest("建立活動發生錯誤.");
            }
        }
    }
}