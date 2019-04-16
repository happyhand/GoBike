using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
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
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberInfoDto memberInfo)
        {
            try
            {
                Tuple<MemberInfoDto, string> result = await this.memberService.GetMemberInfo(memberInfo);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
                return BadRequest("取得會員資訊發生錯誤");
            }
        }
    }
}