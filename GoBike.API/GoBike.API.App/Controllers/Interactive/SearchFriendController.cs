using AutoMapper;
using GoBike.API.App.Filters;
using GoBike.API.App.Models.Member;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Interactive;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Command;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Interactive
{
    /// <summary>
    /// 搜尋好友
    /// </summary>
    [Route("api/friend/[controller]")]
    [ApiController]
    public class SearchFriendController : ApiController
    {
        /// <summary>
        /// memberService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SearchFriendController> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="memberService">memberService</param>
        /// <param name="interactiveService">interactiveService</param>
        public SearchFriendController(ILogger<SearchFriendController> logger, IMapper mapper, IMemberService memberService, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberService = memberService;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberBase">memberBase</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(MemberBaseDto memberBase)
        {
            string memberID = HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResultDto = await this.memberService.GetMemberInfo(memberBase);
                if (responseResultDto.Ok)
                {
                    responseResultDto = await this.interactiveService.SearchFriend(new InteractiveCommandDto() { InitiatorID = memberID, ReceiverID = (responseResultDto.Data as MemberInfoDto).MemberID });
                    if (responseResultDto.Ok)
                    {
                        return Ok(this.mapper.Map<MemberInteractiveViewDto>(responseResultDto.Data));
                    }

                    return BadRequest(responseResultDto.Data);
                }

                return BadRequest(responseResultDto.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Friend Error >>> InitiatorID:{memberID} ReceiverEmail:{memberBase.Email}\n{ex}");
                return BadRequest("搜尋好友發生錯誤.");
            }
        }
    }
}