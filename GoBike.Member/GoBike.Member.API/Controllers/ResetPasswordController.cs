using GoBike.Member.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 重設密碼
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ResetPasswordController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public ResetPasswordController(ILogger<ResetPasswordController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(InputData inputData)
        {
            try
            {
                Tuple<string, string> result = await this.memberService.ResetPassword(inputData.Email);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reset Password Error >>> Email:{inputData.Email}\n{ex}");
                return BadRequest("會員重設密碼發生錯誤.");
            }
        }

        /// <summary>
        /// 請求資料
        /// </summary>
        public class InputData
        {
            /// <summary>
            /// Gets or sets Email
            /// </summary>
            public string Email { get; set; }
        }
    }
}