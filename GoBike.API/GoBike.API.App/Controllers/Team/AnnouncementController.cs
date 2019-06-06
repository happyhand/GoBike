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
        /// 車隊公告功能 - 刪除公告
        /// </summary>
        /// <param name="announcementInfo">announcementInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(AnnouncementInfoDto announcementInfo)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.DeleteAnnouncement(new TeamCommandDto() { TeamID = announcementInfo.TeamID, ExaminerID = memberID, AnnouncementInfo = announcementInfo });
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Announcement Error >>> TeamID:{announcementInfo.TeamID} ExaminerID:{memberID} AnnouncementID:{announcementInfo.AnnouncementID}\n{ex}");
                return BadRequest("刪除公告發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊公告功能 - 編輯公告
        /// </summary>
        /// <param name="announcementInfo">announcementInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(AnnouncementInfoDto announcementInfo)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.EditAnnouncement(new TeamCommandDto() { TeamID = announcementInfo.TeamID, ExaminerID = memberID, AnnouncementInfo = announcementInfo });
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Announcement Error >>> TeamID:{announcementInfo.TeamID} ExaminerID:{memberID} Data:{JsonConvert.SerializeObject(announcementInfo)}\n{ex}");
                return BadRequest("編輯公告發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊公告功能 - 取得公告列表
        /// </summary>
        /// <param name="announcementInfo">announcementInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(AnnouncementInfoDto announcementInfo)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.GetAnnouncementList(new TeamCommandDto() { TeamID = announcementInfo.TeamID, TargetID = memberID });
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Announcement List Error >>> TemaID:{announcementInfo.TeamID} TargetID:{memberID}\n{ex}");
                return BadRequest("取得公告列表發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊公告功能 - 發佈公告
        /// </summary>
        /// <param name="announcementInfo">announcementInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Publish(AnnouncementInfoDto announcementInfo)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.teamService.PublishAnnouncement(new TeamCommandDto() { TeamID = announcementInfo.TeamID, ExaminerID = memberID, AnnouncementInfo = announcementInfo });
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Publish Announcement Error >>> TeamID:{announcementInfo.TeamID} ExaminerID:{memberID} Data:{JsonConvert.SerializeObject(announcementInfo)}\n{ex}");
                return BadRequest("發佈公告發生錯誤.");
            }
        }
    }
}