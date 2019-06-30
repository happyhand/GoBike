using GoBike.Service.Repository.Models.Member;
using GoBike.Service.Service.Interface.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 會員註冊
    /// </summary>
    [Route("api/Member/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<RegisterController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public RegisterController(ILogger<RegisterController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberDto memberDto)
        {
            try
            {
                string result = await this.memberService.Register(memberDto, true);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("會員註冊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Data:{JsonConvert.SerializeObject(memberDto)}\n{ex}");
                return BadRequest("會員註冊發生錯誤.");
            }
        }
    }
}