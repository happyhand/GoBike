using GoBike.API.App.Models.Response;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Response;
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
        public async Task<ResultModel> Post(RequestData requestData)
        {
            RegisterRespone result = await this.memberService.Register(requestData.Email, requestData.Password);
            return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage };
        }

        /// <summary>
        /// 請求參數
        /// </summary>
        public class RequestData
        {
            /// <summary>
            /// Gets or sets Email
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets Password
            /// </summary>
            public string Password { get; set; }
        }
    }
}