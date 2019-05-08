using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Member.Command;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Verifier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 忘記密碼
    /// </summary>
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
        /// POST - 請求產生驗證碼
        /// </summary>
        /// <param name="verifierInfo">verifierInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]")]
        public async Task<IActionResult> GetVerifierCode(VerifierInfoDto verifierInfo)
        {
            try
            {
                verifierInfo.Type = "Password";
                verifierInfo.VerifierCode = await this.verifierService.GetVerifierCode(verifierInfo);
                EmailContext emailContext = EmailContext.GetVerifierCodetEmailContext(verifierInfo.Email, verifierInfo.VerifierCode);
                ResponseResultDto responseResult = await this.verifierService.SendVerifierCode(verifierInfo, emailContext);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Verifier Code Error >>> Data:{JsonConvert.SerializeObject(verifierInfo)}\n{ex}");
                return BadRequest("取得查詢密碼驗證碼發生錯誤.");
            }
        }

        /// <summary>
        /// Post - 驗證驗證碼並重設密碼
        /// </summary>
        /// <param name="verifierInfo">verifierInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/Valid")]
        public async Task<IActionResult> ValidVerifierCode(VerifierInfoDto verifierInfo)
        {
            try
            {
                verifierInfo.Type = "Password";
                bool isValidVerifierCode = await this.verifierService.IsValidVerifierCode(verifierInfo);
                if (!isValidVerifierCode)
                {
                    return BadRequest("驗證碼驗證失敗.");
                }
                else
                {
                    ResponseResultDto responseResult = await this.memberService.ResetPassword(new MemberBaseCommandDto() { Email = verifierInfo.Email });
                    if (responseResult.Ok)
                    {
                        return Ok(responseResult.Data);
                    }

                    return BadRequest(responseResult.Data);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Verifier Code Error >>> Email:{verifierInfo.Email} VerifierCode:{verifierInfo.VerifierCode}\n{ex}");
                return BadRequest("重設密碼驗證發生錯誤.");
            }
        }
    }
}