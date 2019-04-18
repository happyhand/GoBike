using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers.Friend
{
    /// <summary>
    /// 取得好友名單
    /// </summary>
    [Route("api/friend/[controller]")]
    [ApiController]
    public class GetFriendListController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetFriendListController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public GetFriendListController(ILogger<GetFriendListController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST - 取得好友名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> GetFriendList(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                Tuple<IEnumerable<MemberInfoDto>, string> result = await this.interactiveService.GetFriendList(interactiveInfo);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return BadRequest("取得好友名單發生錯誤.");
            }
        }
    }
}