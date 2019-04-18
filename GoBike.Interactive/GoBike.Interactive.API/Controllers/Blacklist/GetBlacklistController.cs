using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers.Blacklist
{
    /// <summary>
    /// 取得黑名單
    /// </summary>
    [Route("api/blacklist/[controller]")]
    [ApiController]
    public class GetBlacklistController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetBlacklistController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public GetBlacklistController(ILogger<GetBlacklistController> logger, IInteractiveService interactiveService)
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