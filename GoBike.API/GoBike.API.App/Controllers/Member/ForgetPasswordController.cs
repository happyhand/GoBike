using GoBike.API.App.Models.Response;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 忘記密碼
    /// </summary>
    [Route("api/member/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ForgetPasswordController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public ForgetPasswordController(ILogger<ForgetPasswordController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <returns>ResultModel</returns>
        [HttpPost]
        public async Task<ResultModel> Post(RequestData requestData)
        {
            ForgetPasswordRespone result = await this.memberService.ForgetPassword(requestData.Email);
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
        }
    }
}