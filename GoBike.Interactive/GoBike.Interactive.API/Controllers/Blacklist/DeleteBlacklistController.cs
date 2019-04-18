using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers.Blacklist
{
    /// <summary>
    /// 刪除黑名單
    /// </summary>
    [Route("api/blacklist/[controller]")]
    [ApiController]
    public class DeleteBlacklistController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<DeleteBlacklistController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public DeleteBlacklistController(ILogger<DeleteBlacklistController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.interactiveService.DeleteBlacklist(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除黑名單成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("刪除黑名單發生錯誤.");
            }
        }
    }
}