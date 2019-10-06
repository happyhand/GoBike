using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員好友功能
    /// </summary>
    [Route("api/Member/[controller]/[action]")]
    [ApiController]
    public class FriendshipController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<FriendshipController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public FriendshipController(ILogger<FriendshipController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// 會員好友 - 取得被加入好友名單
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> GetBeAddFriendList()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.GetBeAddFriendList(memberID);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Be Add Friend List Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("取得被加入好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 會員好友 - 取得黑名單
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> GetBlackList()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.GetBlackList(memberID);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Black List Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("取得黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 會員好友 - 取得好友名單
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> GetFriendList()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.GetFriendList(memberID);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("取得好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 會員好友 - 加入黑名單
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> JoinBlack(JoinBlackPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                InteractiveDto interactiveDto = new InteractiveDto()
                {
                    MemberID = memberID,
                    InteractiveID = postData.MemberID
                };
                ResponseResultDto responseResult = await this.memberService.JoinBlack(interactiveDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Black Error >>> MemberID:{memberID} InteractiveID:{postData.MemberID}\n{ex}");
                return BadRequest("加入黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 會員好友 - 加入好友
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> JoinFriend(JoinFriendPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                InteractiveDto interactiveDto = new InteractiveDto()
                {
                    MemberID = memberID,
                    InteractiveID = postData.MemberID
                };
                ResponseResultDto responseResult = await this.memberService.JoinFriend(interactiveDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Friend Error >>> MemberID:{memberID} InteractiveID:{postData.MemberID}\n{ex}");
                return BadRequest("加入好友發生錯誤.");
            }
        }

        /// <summary>
        /// 會員好友 - 移除黑名單
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> RemoveBlack(RemoveBlackPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                InteractiveDto interactiveDto = new InteractiveDto()
                {
                    MemberID = memberID,
                    InteractiveID = postData.MemberID
                };
                ResponseResultDto responseResult = await this.memberService.RemoveBlack(interactiveDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Black Error >>> MemberID:{memberID} InteractiveID:{postData.MemberID}\n{ex}");
                return BadRequest("移除黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 會員好友 - 移除好友
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> RemoveFriend(RemoveFriendPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                InteractiveDto interactiveDto = new InteractiveDto()
                {
                    MemberID = memberID,
                    InteractiveID = postData.MemberID
                };
                ResponseResultDto responseResult = await this.memberService.RemoveFriend(interactiveDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Friend Error >>> MemberID:{memberID} InteractiveID:{postData.MemberID}\n{ex}");
                return BadRequest("移除好友發生錯誤.");
            }
        }

        /// <summary>
        /// 會員好友 - 搜尋好友
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> SearchFriend(SearchFriendPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                InteractiveDto interactiveDto = new InteractiveDto()
                {
                    MemberID = memberID,
                    SearchKey = postData.Email
                };
                ResponseResultDto responseResult = await this.memberService.SearchFriend(interactiveDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Friend Error >>> MemberID:{memberID} Email:{postData.Email}\n{ex}");
                return BadRequest("搜尋好友發生錯誤.");
            }
        }

        /// <summary>
        /// 加入黑名單 Post 資料
        /// </summary>
        public class JoinBlackPostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string MemberID { get; set; }
        }

        /// <summary>
        /// 加入好友 Post 資料
        /// </summary>
        public class JoinFriendPostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string MemberID { get; set; }
        }

        /// <summary>
        /// 移除黑名單 Post 資料
        /// </summary>
        public class RemoveBlackPostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string MemberID { get; set; }
        }

        /// <summary>
        /// 移除好友 Post 資料
        /// </summary>
        public class RemoveFriendPostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string MemberID { get; set; }
        }

        /// <summary>
        /// 搜尋好友 Post 資料
        /// </summary>
        public class SearchFriendPostData
        {
            /// <summary>
            /// Gets or sets Email
            /// </summary>
            public string Email { get; set; }
        }
    }
}