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
    /// 車隊活動
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class EventController : ControllerBase
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
        /// 建立車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Create(TeamEventDto teamEventDto)
        {
            try
            {
                string result = await this.teamService.CreateTeamEventData(teamEventDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("建立車隊活動資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Event Data Error >>> Data:{JsonConvert.SerializeObject(teamEventDto)}\n{ex}");
                return BadRequest("建立車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 刪除車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(TeamEventDto teamEventDto)
        {
            try
            {
                string result = await this.teamService.DeleteTeamEventData(teamEventDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除車隊活動資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Event Data Error >>> EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return BadRequest("刪除車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 編輯車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(TeamEventDto teamEventDto)
        {
            try
            {
                string result = await this.teamService.EditTeamEventData(teamEventDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("編輯車隊活動資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Event Data Error >>> Data:{JsonConvert.SerializeObject(teamEventDto)}\n{ex}");
                return BadRequest("編輯車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊活動資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(TeamEventDto teamEventDto)
        {
            try
            {
                Tuple<TeamEventDto, string> result = await this.teamService.GetTeamEventData(teamEventDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Event Data Error >>> EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return BadRequest("取得車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊活動資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> GetList(TeamDto teamDto)
        {
            try
            {
                Tuple<IEnumerable<TeamEventDto>, string> result = await this.teamService.GetTeamEventDataList(teamDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Event Data List Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return BadRequest("取得車隊活動資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 加入車隊活動
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Join(TeamEventDto teamEventDto)
        {
            try
            {
                string result = await this.teamService.JoinTeamEvent(teamEventDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入車隊活動成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Team Event Error >>> TeamID:{teamEventDto.TeamID} EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return BadRequest("加入車隊活動發生錯誤.");
            }
        }

        /// <summary>
        /// 離開車隊活動
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Leave(TeamEventDto teamEventDto)
        {
            try
            {
                string result = await this.teamService.LeaveTeamEvent(teamEventDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("離開車隊活動成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Event Error >>> EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return BadRequest("離開車隊活動發生錯誤.");
            }
        }
    }
}