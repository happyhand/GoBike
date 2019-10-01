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
    /// 車隊活動功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class EventController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EventController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public EventController(ILogger<EventController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 車隊活動 - 建立車隊活動資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Create(CreateTeamEventPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamEventDto teamEventDto = new TeamEventDto()
                {
                    TeamID = postData.TeamID,
                    MemberID = memberID,
                    Altitude = postData.Altitude,
                    Distance = postData.Distance,
                    EventDate = postData.EventDate,
                    Title = postData.Title
                };
                ResponseResultDto responseResult = await this.teamService.CreateTeamEventData(teamEventDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Event Data Error >>>  MemberID:{memberID} Data:{JsonConvert.SerializeObject(postData)}\n{ex}");
                return BadRequest("建立車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊活動 - 刪除車隊活動資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Delete(DeleteTeamEventPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamEventDto teamEventDto = new TeamEventDto()
                {
                    TeamID = postData.TeamID,
                    MemberID = memberID,
                    EventID = postData.EventID
                };
                ResponseResultDto responseResult = await this.teamService.DeleteTeamEventData(teamEventDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Event Data Error >>> TeamID:{postData.TeamID} EventID:{postData.EventID} MemberID:{memberID}\n{ex}");
                return BadRequest("刪除車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊活動 - 編輯車隊活動資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Edit(EditTeamEventPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamEventDto teamEventDto = new TeamEventDto()
                {
                    TeamID = postData.TeamID,
                    EventID = postData.EventID,
                    MemberID = memberID,
                    Altitude = postData.Altitude,
                    Distance = postData.Distance,
                    EventDate = postData.EventDate,
                    Title = postData.Title
                };
                ResponseResultDto responseResult = await this.teamService.EditTeamEventData(teamEventDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Event Data Error >>> MemberID:{memberID} Data:{JsonConvert.SerializeObject(postData)}\n{ex}");
                return BadRequest("編輯車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊活動 - 取得車隊活動資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Get(GetTeamEventPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamEventDto teamEventDto = new TeamEventDto()
                {
                    TeamID = postData.TeamID,
                    EventID = postData.EventID,
                    MemberID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.GetTeamEventData(teamEventDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Event Data Error >>> TeamID:{postData.TeamID} EventID:{postData.EventID} MemberID:{memberID}\n{ex}");
                return BadRequest("取得車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊活動 - 加入車隊活動
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Join(JoinTeamEventPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamEventDto teamEventDto = new TeamEventDto()
                {
                    TeamID = postData.TeamID,
                    EventID = postData.EventID,
                    MemberID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.JoinTeamEvent(teamEventDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Team Event Error >>> TeamID:{postData.TeamID} EventID:{postData.EventID} MemberID:{memberID}\n{ex}");
                return BadRequest("加入車隊活動發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊活動 - 離開車隊活動
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Leave(LeaveTeamEventPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamEventDto teamEventDto = new TeamEventDto()
                {
                    EventID = postData.EventID,
                    MemberID = memberID
                };
                ResponseResultDto responseResult = await this.teamService.LeaveTeamEvent(teamEventDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Event Error >>> EventID:{postData.EventID} MemberID:{memberID}\n{ex}");
                return BadRequest("離開車隊活動發生錯誤.");
            }
        }

        /// <summary>
        /// 建立車隊活動 Post 資料
        /// </summary>
        public class CreateTeamEventPostData
        {
            /// <summary>
            /// Gets or sets Altitude
            /// </summary>
            public long Altitude { get; set; }

            /// <summary>
            /// Gets or sets Distance
            /// </summary>
            public long Distance { get; set; }

            /// <summary>
            /// Gets or sets EventDate
            /// </summary>
            public DateTime EventDate { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }

            /// <summary>
            /// Gets or sets Title
            /// </summary>
            public string Title { get; set; }
        }

        /// <summary>
        /// 刪除車隊活動 Post 資料
        /// </summary>
        public class DeleteTeamEventPostData
        {
            /// <summary>
            /// Gets or sets EventID
            /// </summary>
            public string EventID { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }

        /// <summary>
        /// 編輯車隊活動 Post 資料
        /// </summary>
        public class EditTeamEventPostData
        {
            /// <summary>
            /// Gets or sets Altitude
            /// </summary>
            public long Altitude { get; set; }

            /// <summary>
            /// Gets or sets Distance
            /// </summary>
            public long Distance { get; set; }

            /// <summary>
            /// Gets or sets EventDate
            /// </summary>
            public DateTime EventDate { get; set; }

            /// <summary>
            /// Gets or sets EventID
            /// </summary>
            public string EventID { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }

            /// <summary>
            /// Gets or sets Title
            /// </summary>
            public string Title { get; set; }
        }

        /// <summary>
        /// 取得車隊活動 Post 資料
        /// </summary>
        public class GetTeamEventPostData
        {
            /// <summary>
            /// Gets or sets EventID
            /// </summary>
            public string EventID { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }

        /// <summary>
        /// 加入車隊活動 Post 資料
        /// </summary>
        public class JoinTeamEventPostData
        {
            /// <summary>
            /// Gets or sets EventID
            /// </summary>
            public string EventID { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }

        /// <summary>
        /// 離開車隊活動 Post 資料
        /// </summary>
        public class LeaveTeamEventPostData
        {
            /// <summary>
            /// Gets or sets EventID
            /// </summary>
            public string EventID { get; set; }
        }
    }
}