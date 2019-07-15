using GoBike.API.Core.Resource.Enum;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 忘記密碼功能
    /// </summary>
    [ApiController]
    public class ForgetPasswordController : ApiController
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
        /// verifierService
        /// </summary>
        private readonly IVerifierService verifierService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        /// <param name="verifierService">verifierService</param>
        public ForgetPasswordController(ILogger<ForgetPasswordController> logger, IMemberService memberService, IVerifierService verifierService)
        {
            this.logger = logger;
            this.memberService = memberService;
            this.verifierService = verifierService;
        }

        /// <summary>
        /// 忘記密碼 - 請求產生驗證碼
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]")]
        public async Task<IActionResult> GetVerifierCode(ForgetPasswordGetVerifierCodePostData postData)
        {
            try
            {
                string type = VerifierType.ForgetPassword.ToString();
                string verifierCode = await this.verifierService.GetVerifierCode(type, postData.Email);
                if (string.IsNullOrEmpty(verifierCode))
                {
                    this.logger.LogError($"Get Verifier Code Fail For Get Verifier Code >>> Email:{postData.Email}");
                    return BadRequest("取得查詢密碼驗證碼失敗.");
                }

                EmailContext emailContext = EmailContext.GetVerifierCodetEmailContextForForgetPassword(postData.Email, verifierCode);
                ResponseResultDto responseResult = await this.verifierService.SendVerifierCode(type, postData.Email, verifierCode, emailContext);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Verifier Code Error >>> Email:{postData.Email}\n{ex}");
                return BadRequest("請求產生驗證碼發生錯誤.");
            }
        }

        /// <summary>
        /// 忘記密碼 - 驗證驗證碼並重設密碼
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/VerifierCode")]
        public async Task<IActionResult> ResetPassword(ForgetPasswordVerifierPostData postData)
        {
            try
            {
                bool isValidVerifierCode = await this.verifierService.IsValidVerifierCode(VerifierType.ForgetPassword.ToString(), postData.Email, postData.VerifierCode);
                if (!isValidVerifierCode)
                {
                    return BadRequest("驗證碼驗證失敗.");
                }

                ResponseResultDto responseResult = await this.memberService.ResetPassword(postData.Email);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reset Password Error >>> Email:{postData.Email} VerifierCode:{postData.VerifierCode}\n{ex}");
                return BadRequest("會員重設密碼發生錯誤.");
            }
        }

        /// <summary>
        /// 請求產生驗證碼 Post 資料
        /// </summary>
        public class ForgetPasswordGetVerifierCodePostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string Email { get; set; }
        }

        /// <summary>
        /// 驗證驗證碼 Post 資料
        /// </summary>
        public class ForgetPasswordVerifierPostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets VerifierCode
            /// </summary>
            public string VerifierCode { get; set; }
        }
    }
}