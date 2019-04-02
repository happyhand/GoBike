using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員登入
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
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
        public LoginController(ILogger<LoginController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> Post(string email, string password)
        {
            Tuple<LoginInfoDto, string> result = await this.memberService.Login(email, password);
            if (string.IsNullOrEmpty(result.Item2))
            {
                return Ok(result.Item1);
            }

            return BadRequest(result.Item2);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/token")]
        public async Task<IActionResult> Post(string token)
        {
            Tuple<LoginInfoDto, string> result = await this.memberService.Login(token);
            if (string.IsNullOrEmpty(result.Item2))
            {
                return Ok(result.Item1);
            }

            return BadRequest(result.Item2);
        }
    }
}