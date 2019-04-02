using AutoMapper;
using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 取得會員資訊
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GetMemberInfoController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public GetMemberInfoController(ILogger<GetMemberInfoController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Post(string memberID)
        {
            Tuple<MemberInfoDto, string> result = await this.memberService.GetMemberInfo(memberID);
            if (string.IsNullOrEmpty(result.Item2))
            {
                return Ok(result.Item1);
            }

            return BadRequest(result.Item2);
        }
    }
}