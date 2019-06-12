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
    /// 編輯活動
    /// </summary>
    [Route("api/TeamEvent/[controller]")]
    [ApiController]
    public class EditEventController : ControllerBase
    {
        /// <summary>
        /// eventService
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditEventController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="eventService">eventService</param>
        public EditEventController(ILogger<EditEventController> logger, IEventService eventService)
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
                string result = await this.eventService.EditEvent(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("編輯活動成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventInfo:{JsonConvert.SerializeObject(teamCommand.EventInfo)}\n{ex}");
                return BadRequest("編輯活動發生錯誤.");
            }
        }
    }
}