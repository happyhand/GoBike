//using GoBike.API.Core.Applibs;
//using GoBike.API.Core.Resource;
//using GoBike.API.Service.Interface.Team;
//using GoBike.API.Service.Models.Response;
//using GoBike.API.Service.Models.Team.Command;
//using GoBike.API.Service.Models.Team.Command.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.Threading.Tasks;

//namespace GoBike.API.App.Controllersbackup.Team
//{
//    /// <summary>
//    /// 車隊活動功能
//    /// </summary>
//    [Route("api/Team/[controller]/[action]")]
//    [ApiController]
//    public class EventController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<EventController> logger;

//        /// <summary>
//        /// teamService
//        /// </summary>
//        private readonly ITeamService teamService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="teamService">teamService</param>
//        public EventController(ILogger<EventController> logger, ITeamService teamService)
//        {
//            this.logger = logger;
//            this.teamService = teamService;
//        }

//        /// <summary>
//        /// 車隊活動功能 - 取消加入活動
//        /// </summary>
//        /// <param name="eventDetailInfo">eventDetailInfo</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        public async Task<IActionResult> CancelJoin(EventDetailInfoDto eventDetailInfo)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.CreateEvent(new TeamCommandDto() { TeamID = eventDetailInfo.TeamID, TargetID = memberID, EventInfo = eventDetailInfo });
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Cancel Join Event Error >>> TeamID:{eventDetailInfo.TeamID} TargetID:{memberID} EventID:{eventDetailInfo.EventID}\n{ex}");
//                return BadRequest("取消加入活動發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 車隊活動功能 - 建立活動
//        /// </summary>
//        /// <param name="eventDetailInfo">eventDetailInfo</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        public async Task<IActionResult> Create(EventDetailInfoDto eventDetailInfo)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.CreateEvent(new TeamCommandDto() { TeamID = eventDetailInfo.TeamID, TargetID = memberID, EventInfo = eventDetailInfo });
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Create Event Error >>> TeamID:{eventDetailInfo.TeamID} TargetID:{memberID} EventInfo:{JsonConvert.SerializeObject(eventDetailInfo)}\n{ex}");
//                return BadRequest("建立活動發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 車隊活動功能 - 刪除活動
//        /// </summary>
//        /// <param name="eventDetailInfo">eventDetailInfo</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        public async Task<IActionResult> Delete(EventDetailInfoDto eventDetailInfo)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.DeleteEvent(new TeamCommandDto() { TeamID = eventDetailInfo.TeamID, TargetID = memberID, EventInfo = eventDetailInfo });
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Delete Event Error >>> TeamID:{eventDetailInfo.TeamID} TargetID:{memberID} EventID:{eventDetailInfo.EventID}\n{ex}");
//                return BadRequest("刪除活動發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 車隊活動功能 - 編輯活動
//        /// </summary>
//        /// <param name="eventDetailInfo">eventDetailInfo</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        public async Task<IActionResult> Edit(EventDetailInfoDto eventDetailInfo)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.EditEvent(new TeamCommandDto() { TeamID = eventDetailInfo.TeamID, TargetID = memberID, EventInfo = eventDetailInfo });
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Edit Event Error >>> TeamID:{eventDetailInfo.TeamID} TargetID:{memberID} EventInfo:{JsonConvert.SerializeObject(eventDetailInfo)}\n{ex}");
//                return BadRequest("編輯活動發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 車隊活動功能 - 取得活動詳細資訊
//        /// </summary>
//        /// <param name="eventDetailInfo">eventDetailInfo</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        public async Task<IActionResult> Get(EventDetailInfoDto eventDetailInfo)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.GetEventDetailInfo(new TeamCommandDto() { TeamID = eventDetailInfo.TeamID, TargetID = memberID, EventInfo = eventDetailInfo });
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get Event Detail Info Error >>> TeamID:{eventDetailInfo.TeamID} TargetID:{memberID} EventID:{eventDetailInfo.EventID}\n{ex}");
//                return BadRequest("取得活動詳細資訊發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 車隊活動功能 - 加入活動
//        /// </summary>
//        /// <param name="eventDetailInfo">eventDetailInfo</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        public async Task<IActionResult> Join(EventDetailInfoDto eventDetailInfo)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.JoinEvent(new TeamCommandDto() { TeamID = eventDetailInfo.TeamID, TargetID = memberID, EventInfo = eventDetailInfo });
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Join Event Error >>> TeamID:{eventDetailInfo.TeamID} TargetID:{memberID} EventID:{eventDetailInfo.EventID}\n{ex}");
//                return BadRequest("加入活動發生錯誤.");
//            }
//        }
//    }
//}