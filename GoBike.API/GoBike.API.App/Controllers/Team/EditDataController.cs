using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Command;
using GoBike.API.Service.Models.Team.Command.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 車隊編輯
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class EditDataController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditDataController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public EditDataController(ILogger<EditDataController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> GetApplyForRequestList(TeamInfoDto teamInfo)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            TeamCommandDto teamCommand = new TeamCommandDto() { TeamID = teamInfo.TeamID, ExaminerID = memberID, TeamInfo = teamInfo };
            try
            {
                ResponseResultDto responseResult = await this.teamService.EditData(teamCommand);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(teamCommand)}\n{ex}");
                return BadRequest("車隊編輯發生錯誤.");
            }
        }
    }
}