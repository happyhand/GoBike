using GoBike.Service.Service.Interface.Member;
using GoBike.Service.Service.Models.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 會員好友
    /// </summary>
    [ApiController]
    public class FriendshipController : ControllerBase
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
        /// 取得被加入好友名單
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> GetBeAddFriendList(MemberDto memberDto)
        {
            try
            {
                Tuple<IEnumerable<MemberDto>, string> result = await this.memberService.GetBeAddFriendList(memberDto.MemberID);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Be Add Friend List Error >>> MemberID:{memberDto.MemberID}\n{ex}");
                return BadRequest("取得被加入好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> GetBlackList(MemberDto memberDto)
        {
            try
            {
                Tuple<IEnumerable<MemberDto>, string> result = await this.memberService.GetBlackList(memberDto.MemberID);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Black List Error >>> MemberID:{memberDto.MemberID}\n{ex}");
                return BadRequest("取得黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> GetFriendList(MemberDto memberDto)
        {
            try
            {
                this.logger.LogInformation($"Get Friend List >>> MemberID:{memberDto.MemberID}");
                Tuple<IEnumerable<MemberDto>, string> result = await this.memberService.GetFriendList(memberDto.MemberID);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> MemberID:{memberDto.MemberID}\n{ex}");
                return BadRequest("取得好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> JoinBlack(InteractiveDto interactiveDto)
        {
            try
            {
                string result = await this.memberService.JoinBlack(interactiveDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入黑名單成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Black Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return BadRequest("加入黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> JoinFriend(InteractiveDto interactiveDto)
        {
            try
            {
                string result = await this.memberService.JoinFriend(interactiveDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("加入好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Friend Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return BadRequest("加入好友發生錯誤.");
            }
        }

        /// <summary>
        /// 移除黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> RemoveBlack(InteractiveDto interactiveDto)
        {
            try
            {
                string result = await this.memberService.RemoveBlack(interactiveDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("移除黑名單成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Black Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return BadRequest("移除黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> RemoveFriend(InteractiveDto interactiveDto)
        {
            try
            {
                string result = await this.memberService.RemoveFriend(interactiveDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("移除好友成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Friend Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return BadRequest("移除好友發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> SearchFriend(InteractiveDto interactiveDto)
        {
            try
            {
                Tuple<MemberDto, string> result = await this.memberService.SearchFriend(interactiveDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Friend Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return BadRequest("搜尋好友發生錯誤.");
            }
        }
    }
}