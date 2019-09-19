using GoBike.Service.Service.Interface.Team;
using GoBike.Service.Service.Models.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Team
{
    /// <summary>
    /// 車隊公告
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
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
        /// 建立車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Create(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                string result = await this.teamService.CreateTeamAnnouncementData(teamAnnouncementDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("建立車隊公告資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementDto)}\n{ex}");
                return BadRequest("建立車隊公告資料發生錯誤.");
            }
        }

        /// <summary>
        /// 刪除車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                string result = await this.teamService.DeleteTeamAnnouncementData(teamAnnouncementDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除車隊公告資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Announcement Data Error >>> TeamID:{teamAnnouncementDto.TeamID} AnnouncementID:{teamAnnouncementDto.AnnouncementID} MemberID:{teamAnnouncementDto.MemberID}\n{ex}");
                return BadRequest("刪除車隊公告資料發生錯誤.");
            }
        }

        /// <summary>
        /// 編輯車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                string result = await this.teamService.EditTeamAnnouncementData(teamAnnouncementDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("編輯車隊公告資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementDto)}\n{ex}");
                return BadRequest("編輯車隊公告資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊公告資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(TeamDto teamDto)
        {
            try
            {
                Tuple<IEnumerable<TeamAnnouncementDto>, string> result = await this.teamService.GetTeamAnnouncementDataList(teamDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Announcement Data List Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return BadRequest("取得車隊公告資料列表發生錯誤.");
            }
        }
    }
}