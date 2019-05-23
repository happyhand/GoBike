using GoBike.Member.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 驗證會員
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyMemberController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<VerifyMemberController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public VerifyMemberController(ILogger<VerifyMemberController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(IEnumerable<string> memberIDs)
        {
            try
            {
                string result = await this.memberService.VerifyMemberList(memberIDs);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("有效會員.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Verify Member Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return BadRequest("驗證會員發生錯誤.");
            }
        }
    }
}