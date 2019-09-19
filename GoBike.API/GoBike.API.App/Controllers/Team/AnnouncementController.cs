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
    /// 車隊公告功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class AnnouncementController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AnnouncementController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public AnnouncementController(ILogger<AnnouncementController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 車隊公告 - 建立車隊公告資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Create(CreateTeamAnnouncementPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamAnnouncementDto teamAnnouncementDto = new TeamAnnouncementDto()
                {
                    TeamID = postData.TeamID,
                    MemberID = memberID,
                    Context = postData.Context,
                    LimitDate = postData.LimitDate
                };
                ResponseResultDto responseResult = await this.teamService.CreateTeamAnnouncementData(teamAnnouncementDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Announcement Data Error >>>  MemberID:{memberID} Data:{JsonConvert.SerializeObject(postData)}\n{ex}");
                return BadRequest("建立車隊公告資料發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊公告 - 刪除車隊公告資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Delete(DeleteTeamAnnouncementPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamAnnouncementDto teamAnnouncementDto = new TeamAnnouncementDto()
                {
                    TeamID = postData.TeamID,
                    MemberID = memberID,
                    AnnouncementID = postData.AnnouncementID
                };
                ResponseResultDto responseResult = await this.teamService.DeleteTeamAnnouncementData(teamAnnouncementDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Announcement Data Error >>> TeamID:{postData.TeamID} MemberID:{memberID} AnnouncementID:{postData.AnnouncementID}\n{ex}");
                return BadRequest("刪除車隊公告資料發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊公告 - 編輯車隊公告資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Edit(EditTeamAnnouncementPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamAnnouncementDto teamAnnouncementDto = new TeamAnnouncementDto()
                {
                    TeamID = postData.TeamID,
                    MemberID = memberID,
                    AnnouncementID = postData.AnnouncementID,
                    Context = postData.Context
                };
                ResponseResultDto responseResult = await this.teamService.EditTeamAnnouncementData(teamAnnouncementDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Announcement Data Error >>> TeamID:{postData.TeamID} MemberID:{memberID} AnnouncementID:{postData.AnnouncementID} Context:{postData.Context}\n{ex}");
                return BadRequest("編輯車隊公告資料發生錯誤.");
            }
        }

        /// <summary>
        /// 建立車隊公告 Post 資料
        /// </summary>
        public class CreateTeamAnnouncementPostData
        {
            /// <summary>
            /// Gets or sets Context
            /// </summary>
            public string Context { get; set; }

            /// <summary>
            /// Gets or sets LimitDate
            /// </summary>
            public int LimitDate { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }

        /// <summary>
        /// 刪除車隊公告 Post 資料
        /// </summary>
        public class DeleteTeamAnnouncementPostData
        {
            /// <summary>
            /// Gets or sets AnnouncementID
            /// </summary>
            public string AnnouncementID { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }

        /// <summary>
        /// 編輯車隊公告 Post 資料
        /// </summary>
        public class EditTeamAnnouncementPostData
        {
            /// <summary>
            /// Gets or sets AnnouncementID
            /// </summary>
            public string AnnouncementID { get; set; }

            /// <summary>
            /// Gets or sets Context
            /// </summary>
            public string Context { get; set; }

            /// <summary>
            /// Gets or sets TeamID
            /// </summary>
            public string TeamID { get; set; }
        }
    }
}