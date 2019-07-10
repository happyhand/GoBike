using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Core.Resource.Enum;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員綁定行動電話功能
    /// </summary>
    [Route("api/Member/[controller]/[action]")]
    [ApiController]
    public class MobileBindController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<MobileBindController> logger;

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
        public MobileBindController(ILogger<MobileBindController> logger, IMemberService memberService, IVerifierService verifierService)
        {
            this.logger = logger;
            this.memberService = memberService;
            this.verifierService = verifierService;
        }

        /// <summary>
        /// 會員綁定行動電話 - 驗證驗證碼並綁定行動電話
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Bind(MobileBindVerifierPostData postData)
        {
            try
            {
                bool isValidVerifierCode = await this.verifierService.IsValidVerifierCode(VerifierType.MobileBind.ToString(), postData.Email, postData.VerifierCode);
                if (!isValidVerifierCode)
                {
                    return BadRequest("驗證碼驗證失敗.");
                }
                string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
                MemberDto memberDto = new MemberDto()
                {
                    MemberID = memberID,
                    Mobile = postData.Mobile,
                    MoblieBindType = (int)MoblieBindType.Bind
                };
                ResponseResultDto responseResult = await this.memberService.EditData(memberDto);
                if (responseResult.Ok)
                {
                    return Ok("綁定行動電話成功.");
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reset Password Error >>> Email:{postData.Email} VerifierCode:{postData.VerifierCode}\n{ex}");
                return BadRequest("會員綁定行動電話發生錯誤.");
            }
        }

        /// <summary>
        /// 會員綁定行動電話 - 請求產生驗證碼
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> GetVerifierCode(MobileBindGetVerifierCodePostData postData)
        {
            try
            {
                string type = VerifierType.MobileBind.ToString();
                string verifierCode = await this.verifierService.GetVerifierCode(type, postData.Email);
                if (string.IsNullOrEmpty(verifierCode))
                {
                    this.logger.LogError($"Get Verifier Code Fail For Get Verifier Code >>> Email:{postData.Email}");
                    return BadRequest("取得綁定行動電話驗證碼失敗.");
                }

                EmailContext emailContext = EmailContext.GetVerifierCodetEmailContextForMobileBind(postData.Email, verifierCode);
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
        /// 請求產生驗證碼 Post 資料
        /// </summary>
        public class MobileBindGetVerifierCodePostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string Email { get; set; }
        }

        /// <summary>
        /// 驗證驗證碼 Post 資料
        /// </summary>
        public class MobileBindVerifierPostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets Mobile
            /// </summary>
            public string Mobile { get; set; }

            /// <summary>
            /// Gets or sets VerifierCode
            /// </summary>
            public string VerifierCode { get; set; }
        }
    }
}