using GoBike.Member.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員註冊
    /// </summary>
    [Route("api/member/[controller]")]
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
        /// <param name="requestData">requestData</param>
        /// <returns>ResultModel</returns>
        [HttpPost]
        public async Task<IActionResult> Post(string email, string password)
        {
            string result = await this.memberService.Register(email, password);
            if (string.IsNullOrEmpty(result))
            {
                return Ok("Register success.");
            }

            return BadRequest(result);
        }
    }
}