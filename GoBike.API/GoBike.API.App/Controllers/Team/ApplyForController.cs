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
    /// 申請加入功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class ApplyForController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ApplyForController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public ApplyForController(ILogger<ApplyForController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 申請加入 - 允許申請加入
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> AllowJoin(AllowApplyForJoinPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    TeamID = postData.TeamID,
                    ExaminerID = memberID,
                    TargetID = postData.TargetID
                };
                ResponseResultDto responseResult = await this.teamService.AllowApplyForJoinTeam(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Allow Apply For Join Team Error >>> TeamID:{postData.TeamID} ExaminerID:{memberID} TargetID:{postData.TargetID}\n{ex}");
                return BadRequest("加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 申請加入 - 拒絕申請加入
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> RejectJoin(RejectApplyForJoinPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    TeamID = postData.TeamID,
                    ExaminerID = memberID,
                    TargetID = postData.TargetID
                };
                ResponseResultDto responseResult = await this.teamService.RejectApplyForJoinTeam(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TeamID:{postData.TeamID} ExaminerID:{memberID} TargetID:{postData.TargetID}\n{ex}");
                return BadRequest("拒絕申請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 申請加入 - 請求加入
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> RequestJoin(RequestApplyForJoinPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    TeamID = postData.TeamID,
                    ExecutorID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.ApplyForJoinTeam(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Request Apply For Join Team Error >>> TeamID:{postData.TeamID} ExecutorID:{memberID}\n{ex}");
                return BadRequest("申請加入車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 允許申請加入 Post 資料
        /// </summary>
        public class AllowApplyForJoinPostData
        {
            /// <summary>
            /// Gets or sets TargetID
            /// </summary>
            public string TargetID { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }

        /// <summary>
        /// 拒絕申請加入 Post 資料
        /// </summary>
        public class RejectApplyForJoinPostData
        {
            /// <summary>
            /// Gets or sets TargetID
            /// </summary>
            public string TargetID { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }

        /// <summary>
        /// 請求加入 Post 資料
        /// </summary>
        public class RequestApplyForJoinPostData
        {
            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }
    }
}