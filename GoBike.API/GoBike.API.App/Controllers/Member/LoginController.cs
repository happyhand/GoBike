using GoBike.API.Core.Applibs;
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
    /// 會員登入功能
    /// </summary>
    [ApiController]
    public class LoginController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<LoginController> logger;

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
        /// 會員登入 - 自動登入
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> AutoToken(AutoTokenLoginPostData postData)
        {
            try
            {
                ResponseResultDto responseResult = await this.memberService.Login(postData.Token);
                return await this.HandleLoginResult(responseResult);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Auto Token Error >>> Token:{postData.Token}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入 - FB 登入
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> FB(FBLoginPostData postData)
        {
            try
            {
                ResponseResultDto responseResult = await this.memberService.LoginFB(postData.Email, postData.Token);
                return await this.HandleLoginResult(responseResult);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login FB Error >>> FBToken:{postData.Token}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入 - Google 登入
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> Google(FBLoginPostData postData)
        {
            try
            {
                ResponseResultDto responseResult = await this.memberService.LoginGoogle(postData.Email, postData.Token);
                return await this.HandleLoginResult(responseResult);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Google Error >>> GoogleToken:{postData.Token}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入 - 一般登入
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]")]
        public async Task<IActionResult> Normal(NormalLoginPostData postData)
        {
            try
            {
                ResponseResultDto responseResult = await this.memberService.Login(postData.Email, postData.Password);
                return await this.HandleLoginResult(responseResult);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Normal Login Error >>> Email:{postData.Email} Password:{postData.Password}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 處理登入結果
        /// </summary>
        /// <param name="responseResult">responseResult</param>
        /// <returns>IActionResult</returns>
        private async Task<IActionResult> HandleLoginResult(ResponseResultDto responseResult)
        {
            if (responseResult.Ok)
            {
                string[] dataArr = responseResult.Data;
                string memberID = dataArr[0];
                string token = dataArr[1];
                this.HttpContext.Session.SetObject(CommonFlagHelper.CommonFlag.SessionFlag.MemberID, memberID);
                ResponseResultDto recordSessionIDResult = await this.memberService.RecordSessionID(memberID, this.HttpContext.Session.Id);
                if (recordSessionIDResult.Ok)
                {
                    return Ok(token);
                }
                else
                {
                    return BadRequest("會員登入失敗.");
                }
            }

            return BadRequest(responseResult.Data);
        }

        /// <summary>
        /// 自動登入 Post 資料
        /// </summary>
        public class AutoTokenLoginPostData
        {
            /// <summary>
            /// Gets or sets Token
            /// </summary>
            public string Token { get; set; }
        }

        /// <summary>
        /// FB 登入 Post 資料
        /// </summary>
        public class FBLoginPostData
        {
            /// <summary>
            /// Gets or sets Email
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets Token
            /// </summary>
            public string Token { get; set; }
        }

        /// <summary>
        /// Google 登入 Post 資料
        /// </summary>
        public class GoogleLoginPostData
        {
            /// <summary>
            /// Gets or sets Email
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets Token
            /// </summary>
            public string Token { get; set; }
        }

        /// <summary>
        /// 一般登入 Post 資料
        /// </summary>
        public class NormalLoginPostData
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