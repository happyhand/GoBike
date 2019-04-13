using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers
{
    /// <summary>
    /// 黑名單
    /// </summary>
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<BlacklistController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public BlacklistController(ILogger<BlacklistController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST - 加入黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/add")]
        public async Task<IActionResult> AddBlacklist(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.interactiveService.AddBlacklist(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("已加入黑名單.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("加入黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 刪除黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/delete")]
        public async Task<IActionResult> DeleteBlacklist(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.interactiveService.DeleteBlacklist(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("已刪除黑名單.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("刪除黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 取得黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/get")]
        public async Task<IActionResult> GetBlacklist(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                Tuple<IEnumerable<MemberInfoDto>, string> result = await this.interactiveService.GetBlacklist(interactiveInfo);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return BadRequest("取得黑名單發生錯誤.");
            }
        }
    }
}