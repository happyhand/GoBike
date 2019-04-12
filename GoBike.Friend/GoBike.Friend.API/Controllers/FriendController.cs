using GoBike.Friend.Service.Interface;
using GoBike.Friend.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Friend.API.Controllers
{
    /// <summary>
    /// 好友
    /// </summary>
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
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/add")]
        public async Task<IActionResult> AddFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.interactiveService.AddFriend(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("加入好友發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/request")]
        public async Task<IActionResult> AddFriendRequest(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.interactiveService.AddFriendRequest(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("發送加入好友請求成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("加入好友請求發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 刪除好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/delete")]
        public async Task<IActionResult> DeleteFriend(InteractiveInfoDto interactiveInfo)
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

        /// <summary>
        /// POST - 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/deleteRequest")]
        public async Task<IActionResult> DeleteRequestForAddFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.interactiveService.DeleteRequestForAddFriend(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("刪除加入好友請求成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("刪除加入好友請求發生錯誤.");
            }
        }

        /// <summary>
        /// POST - 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/requestList")]
        public async Task<IActionResult> GetAddFriendRequestList(InteractiveInfoDto interactiveInfo)
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

        /// <summary>
        /// POST - 取得好友名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/list")]
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

        /// <summary>
        /// POST - 拒絕加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/reject")]
        public async Task<IActionResult> RejectBeFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string result = await this.interactiveService.RejectBeFriend(interactiveInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("拒絕加入好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return BadRequest("拒絕加入好友發生錯誤.");
            }
        }
    }
}