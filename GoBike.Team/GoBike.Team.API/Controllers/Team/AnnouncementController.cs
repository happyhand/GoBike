using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 車隊公告功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        /// <summary>
        /// announcementService
        /// </summary>
        private readonly IAnnouncementService announcementService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AnnouncementController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="announcementService">announcementService</param>
        public AnnouncementController(ILogger<AnnouncementController> logger, IAnnouncementService announcementService)
        {
            this.logger = logger;
            this.announcementService = announcementService;
        }

        /// <summary>
        /// POST - 刪除公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.announcementService.DeleteAnnouncement(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除公告成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Announcement Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementID:{(teamCommand.AnnouncementInfo != null ? teamCommand.AnnouncementInfo.AnnouncementID : "Null")}\n{ex}");
                return BadRequest("刪除公告發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 編輯公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.announcementService.EditAnnouncement(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("編輯公告成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Announcement Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementInfo:{JsonConvert.SerializeObject(teamCommand.AnnouncementInfo)}\n{ex}");
                return BadRequest("編輯公告發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 取得公告列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(TeamCommandDto teamCommand)
        {
            try
            {
                Tuple<IEnumerable<AnnouncementInfoDto>, string> result = await this.announcementService.GetAnnouncementList(teamCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Announcement List Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("取得公告列表發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 發佈公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Publish(TeamCommandDto teamCommand)
        {
            try
            {
                string result = await this.announcementService.PublishAnnouncement(teamCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("發佈公告成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Publish Announcement Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementInfo:{JsonConvert.SerializeObject(teamCommand.AnnouncementInfo)}\n{ex}");
                return BadRequest("發佈公告發生錯誤.");
            }
        }
    }
}