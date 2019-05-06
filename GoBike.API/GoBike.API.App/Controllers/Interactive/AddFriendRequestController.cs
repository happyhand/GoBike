using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Command;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Interactive
{
    /// <summary>
    /// 加入好友請求
    /// </summary>
    [Route("api/Friend/[controller]")]
    [ApiController]
    public class AddFriendRequestController : ApiController
    {
        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AddFriendRequestController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public AddFriendRequestController(ILogger<AddFriendRequestController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(MemberBaseCommandDto memberBaseCommand)
        {
            string memberID = HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.AddFriendRequest(new MemberInteractiveCommandDto() { InitiatorID = memberID, ReceiverID = memberBaseCommand.MemberID });
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{memberID} ReceiverID:{memberBaseCommand.MemberID}\n{ex}");
                return BadRequest("加入好友請求發生錯誤.");
            }
        }
    }
}