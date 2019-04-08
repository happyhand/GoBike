using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using GoBikeAPI.App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員登入
    /// </summary>
    [ApiController]
    public class LoginController : ApiController
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
        /// POST - Normal Login
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]")]
        public async Task<IActionResult> NormalLogin(InputData inputData)
        {
            try
            {
                ResponseResultDto responseResultDto = await this.memberService.Login(inputData.Email, inputData.Password);
                return this.ResponseResultHandler(responseResultDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Normal Login Error >>> Email:{inputData.Email} Password:{inputData.Password}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// POST - Token Login
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]/token")]
        public async Task<IActionResult> TokenLogin(InputData inputData)
        {
            try
            {
                ResponseResultDto responseResultDto = await this.memberService.Login(inputData.Token);
                return this.ResponseResultHandler(responseResultDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Token Login Error >>> Token:{inputData.Token}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 處理回應資料
        /// </summary>
        /// <param name="responseResultDto">responseResultDto</param>
        /// <returns>IActionResult</returns>
        private IActionResult ResponseResultHandler(ResponseResultDto responseResultDto)
        {
            if (responseResultDto.Ok)
            {
                LoginInfoDto loginInfoDto = responseResultDto.Data as LoginInfoDto;
                this.HttpContext.Session.SetObject(CommonFlagHelper.CommonFlag.SessionFlag.MemberID, loginInfoDto.MemberID);
                return Ok(loginInfoDto.Token);
            }

            return BadRequest(responseResultDto.Data);
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