using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers.Friend
{
    /// <summary>
    /// 刪除好友
    /// </summary>
    [Route("api/friend/[controller]")]
    [ApiController]
    public class DeleteFriendController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<DeleteFriendController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public DeleteFriendController(ILogger<DeleteFriendController> logger, IInteractiveService interactiveService)
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
                string result = await this.interactiveService.DeleteFriend(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("刪除好友發生錯誤.");
            }
        }
    }
}