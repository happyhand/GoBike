using GoBike.Service.Repository.Models.Team;
using GoBike.Service.Service.Interface.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 編輯車隊資料
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class EditTeamDataController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditTeamDataController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public EditTeamDataController(ILogger<EditTeamDataController> logger, ITeamService teamService)
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
                string result = await this.teamService.EditTeamData(teamDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("編輯車隊資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Data Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return BadRequest("編輯車隊資料發生錯誤.");
            }
        }
    }
}