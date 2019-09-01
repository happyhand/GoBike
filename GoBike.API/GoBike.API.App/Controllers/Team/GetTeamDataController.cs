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
    /// 取得車隊資料
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class GetTeamDataController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetTeamDataController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetTeamDataController(ILogger<GetTeamDataController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(GetTeamPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    TeamID = postData.TeamID,
                    ExecutorID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.GetTeamData(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data Error >>> TeamID:{postData.TeamID} ExecutorID:{memberID}\n{ex}");
                return BadRequest("取得車隊資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊資料 Post 資料
        /// </summary>
        public class GetTeamPostData
        {
            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }
    }
}