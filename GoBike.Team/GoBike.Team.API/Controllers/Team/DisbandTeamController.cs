using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 解散車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class DisbandTeamController : ControllerBase
    {
        /// <summary>
        /// announcementService
        /// </summary>
        private readonly IAnnouncementService announcementService;

        /// <summary>
        /// eventService
        /// </summary>
        private readonly IEventService eventService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<DisbandTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        /// <param name="announcementService">announcementService</param>
        /// <param name="eventService">eventService</param>
        public DisbandTeamController(ILogger<DisbandTeamController> logger, ITeamService teamService, IAnnouncementService announcementService, IEventService eventService)
        {
            this.logger = logger;
            this.teamService = teamService;
            this.announcementService = announcementService;
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
                string result = await this.announcementService.DeleteAnnouncementListOfTeam(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    result = await this.eventService.DeleteEventListOfTeam(teamCommand);
                    if (string.IsNullOrEmpty(result))
                    {
                        result = await this.teamService.DisbandTeam(teamCommand);
                        if (string.IsNullOrEmpty(result))
                        {
                            return Ok("解散車隊成功.");
                        }
                    }
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Disband Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}\n{ex}");
                return BadRequest("解散車隊發生錯誤.");
            }
        }
    }
}