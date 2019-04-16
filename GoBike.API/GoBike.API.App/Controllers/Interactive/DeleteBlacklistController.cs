using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interactive;
using GoBike.API.Service.Interface.Interactive;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Interactive
{
    /// <summary>
    /// 刪除黑名單
    /// </summary>
    [Route("api/blacklist/[controller]")]
    [ApiController]
    public class DeleteBlacklistController : ApiController
    {
        /// <summary>
        /// memberService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public DeleteBlacklistController(ILogger<GetFriendListController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(MemberInfoDto memberInfo)
        {
            string memberID = HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResultDto = await this.interactiveService.DeleteBlacklist(new InteractiveInfoDto() { InitiatorID = memberID, PassiveID = memberInfo.MemberID });
                if (responseResultDto.Ok)
                {
                    return Ok(responseResultDto.Data);
                }

                return BadRequest(responseResultDto.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{memberID} PassiveID:{memberInfo.MemberID}\n{ex}");
                return BadRequest("刪除黑名單發生錯誤.");
            }
        }
    }
}