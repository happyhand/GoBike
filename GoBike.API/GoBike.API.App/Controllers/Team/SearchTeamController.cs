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
    /// 搜尋車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class SearchTeamController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SearchTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public SearchTeamController(ILogger<SearchTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(SearchTeamPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    SearchKey = postData.SearchKey,
                    ExecutorID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.SearchTeam(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Team Error >>> ExecutorID:{memberID} SearchKey:{postData.SearchKey}\n{ex}");
                return BadRequest("搜尋車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋車隊 Post 資料
        /// </summary>
        public class SearchTeamPostData
        {
            /// <summary>
            /// Gets or sets SearchKey
            /// </summary>
            public string SearchKey { get; set; }
        }
    }
}