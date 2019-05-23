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
    /// 好友功能
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<FriendController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public FriendController(ILogger<FriendController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST - 加入好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Add(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string result = await this.interactiveService.AddFriend(interactiveCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return BadRequest("加入好友發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 刪除好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string result = await this.interactiveService.DeleteFriend(interactiveCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return BadRequest("刪除好友發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 取得好友名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                Tuple<IEnumerable<string>, string> result = await this.interactiveService.GetFriendList(interactiveCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
                return BadRequest("取得好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 拒絕加入好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Reject(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string result = await this.interactiveService.RejectBeFriend(interactiveCommand);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("拒絕加入好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return BadRequest("拒絕加入好友發生錯誤.");
            }
        }
    }
}