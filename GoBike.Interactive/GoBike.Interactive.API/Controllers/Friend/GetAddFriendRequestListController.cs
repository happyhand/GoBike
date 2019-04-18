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
    /// 取得加入好友請求名單
    /// </summary>
    [Route("api/friend/[controller]")]
    [ApiController]
    public class GetAddFriendRequestListController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetAddFriendRequestListController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public GetAddFriendRequestListController(ILogger<GetAddFriendRequestListController> logger, IInteractiveService interactiveService)
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
                Tuple<IEnumerable<MemberInfoDto>, string> result = await this.interactiveService.GetAddFriendRequestList(interactiveInfo);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return BadRequest("取得加入好友請求名單發生錯誤.");
            }
        }
    }
}