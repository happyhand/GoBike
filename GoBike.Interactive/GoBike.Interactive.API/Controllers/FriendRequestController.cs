using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers
{
    /// <summary>
    /// 好友請求功能
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<FriendRequestController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public FriendRequestController(ILogger<FriendRequestController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST - 加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Add(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string result = await this.interactiveService.AddFriendRequest(interactiveCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入好友請求成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return BadRequest("加入好友請求發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string result = await this.interactiveService.DeleteRequestForAddFriend(interactiveCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除加入好友請求成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return BadRequest("刪除加入好友請求發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                Tuple<IEnumerable<string>, string> result = await this.interactiveService.GetAddFriendRequestList(interactiveCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
                return BadRequest("取得加入好友請求名單發生錯誤.");
            }
        }
    }
}