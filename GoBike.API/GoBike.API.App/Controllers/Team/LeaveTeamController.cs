using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 離開車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class LeaveTeamController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<LeaveTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public LeaveTeamController(ILogger<LeaveTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(LeaveTeamPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    TeamID = postData.TeamID,
                    ExecutorID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.LeaveTeam(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TeamID:{postData.TeamID} ExecutorID:{memberID}\n{ex}");
                return BadRequest("離開車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 離開車隊 Post 資料
        /// </summary>
        public class LeaveTeamPostData
        {
            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }
    }
}