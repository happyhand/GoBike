using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 會員註冊
    /// </summary>
    [Route("api/[controller]")]
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
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberInfoDto memberInfo)
        {
            try
            {
                string result = await this.memberService.Register(memberInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("會員註冊成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Email:{memberInfo.Email} Password:{memberInfo.Password}\n{ex}");
                return BadRequest("會員註冊發生錯誤.");
            }
        }
    }
}