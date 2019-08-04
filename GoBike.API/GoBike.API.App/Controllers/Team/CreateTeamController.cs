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
    /// 建立車隊
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class CreateTeamController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<CreateTeamController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public CreateTeamController(ILogger<CreateTeamController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(CreateTeamPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            TeamDto teamDto = new TeamDto()
            {
                TeamName = postData.TeamName,
                CityID = postData.CityID,
                TeamInfo = postData.TeamInfo,
                SearchStatus = postData.SearchStatus,
                ExamineStatus = postData.ExamineStatus,
                FrontCoverUrl = postData.FrontCoverUrl,
                PhotoUrl = postData.PhotoUrl,
                ExecutorID = memberID
            };

            try
            {
                ResponseResultDto responseResult = await this.teamService.CreateTeam(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return BadRequest("建立車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 建立車隊 Post 資料
        /// </summary>
        public class CreateTeamPostData
        {
            /// <summary>
            /// Gets or sets CityID
            /// </summary>
            public int CityID { get; set; }

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
            /// Gets or sets TeamInfo
            /// </summary>
            public string TeamInfo { get; set; }

            /// <summary>
            /// Gets or sets TeamName
            /// </summary>
            public string TeamName { get; set; }
        }
    }
}