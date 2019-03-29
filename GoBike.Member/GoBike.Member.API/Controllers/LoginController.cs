using GoBike.API.App.Applibs;
using GoBike.API.App.Models.Response;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員登入
    /// </summary>
    [Route("api/member/[controller]")]
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
        /// <param name="requestData">requestData</param>
        /// <returns>ResultModel</returns>
        [HttpPost]
        public async Task<ResultModel> Post(RequestData requestData)
        {
            try
            {
                LoginRespone result;
                if (string.IsNullOrEmpty(requestData.Token))
                {
                    result = await this.memberService.Login(requestData.Email, requestData.Password);
                }
                else
                {
                    result = await this.memberService.Login(requestData.Token);
                }

                if (result.ResultCode == 1)
                {
                    this.HttpContext.Session.SetObject(Utility.Session_MemberID, result.MemberID);
                }

                return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage, ResultData = result.Token };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Error >>> Email:{requestData.Email} Password:{requestData.Password} Token:{requestData.Token} \n{ex}");
                return new ResultModel() { ResultCode = -999, ResultMessage = "Login Error" };
            }
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

            /// <summary>
            /// Gets or sets Token
            /// </summary>
            public string Token { get; set; }
        }
    }
}