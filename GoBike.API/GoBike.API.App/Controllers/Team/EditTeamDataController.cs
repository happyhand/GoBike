using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 編輯車隊資料
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class EditTeamDataController : ApiController
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
        /// 編輯車隊資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(EditTeamPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    TeamID = postData.TeamID,
                    TeamInfo = postData.TeamInfo,
                    SearchStatus = postData.SearchStatus,
                    ExamineStatus = postData.ExamineStatus,
                    FrontCoverUrl = postData.FrontCoverUrl,
                    PhotoUrl = postData.PhotoUrl,
                    ExecutorID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.EditTeamData(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Data Error >>> PostData:{JsonConvert.SerializeObject(postData)}\n{ex}");
                return BadRequest("編輯車隊資料發生錯誤.");
            }
        }

        /// <summary>
        /// 編輯車隊資料 Post 資料
        /// </summary>
        public class EditTeamPostData
        {
            /// <summary>
            /// Gets or sets ExamineStatus
            /// </summary>
            public int ExamineStatus { get; set; }

            /// <summary>
            /// Gets or sets FrontCoverUrl
            /// </summary>
            public string FrontCoverUrl { get; set; }

            /// <summary>
            /// Gets or sets PhotoUrl
            /// </summary>
            public string PhotoUrl { get; set; }

            /// <summary>
            /// Gets or sets SearchStatus
            /// </summary>
            public int SearchStatus { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }

            /// <summary>
            /// Gets or sets TeamInfo
            /// </summary>
            public string TeamInfo { get; set; }
        }
    }
}