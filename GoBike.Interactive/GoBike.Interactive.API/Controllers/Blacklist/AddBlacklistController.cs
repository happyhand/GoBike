using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers.Blacklist
{
    /// <summary>
    /// 加入黑名單
    /// </summary>
    [Route("api/blacklist/[controller]")]
    [ApiController]
    public class AddBlacklistController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AddBlacklistController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public AddBlacklistController(ILogger<AddBlacklistController> logger, IInteractiveService interactiveService)
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
                string result = await this.interactiveService.AddBlacklist(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入黑名單成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("加入黑名單發生錯誤.");
            }
        }
    }
}