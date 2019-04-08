using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        /// POST - Request Get Verifier Code
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]")]
        public async Task<IActionResult> GetVerifierCode(InputData inputData)
        {
            try
            {
                ResponseResultDto responseResultDto = await this.verifierService.SendVerifierCode(inputData.Email);
                if (responseResultDto.Ok)
                {
                    return Ok(responseResultDto.Data);
                }

                return BadRequest(responseResultDto.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Verifier Code Error >>> Email:{inputData.Email}\n{ex}");
                return BadRequest("取得查詢密碼驗證碼發生錯誤.");
            }
        }

        /// <summary>
        /// Post - Valid Verifier Code and Get New Password
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]/valid")]
        public async Task<IActionResult> ValidVerifierCode(InputData inputData)
        {
            try
            {
                bool isValidVerifierCode = await this.verifierService.IsValidVerifierCode(inputData.Email, inputData.VerifierCode);
                if (!isValidVerifierCode)
                {
                    return BadRequest("驗證碼驗證失敗.");
                }
                else
                {
                    ResponseResultDto responseResultDto = await this.memberService.ResetPassword(inputData.Email);
                    if (responseResultDto.Ok)
                    {
                        return Ok(responseResultDto.Data);
                    }

                    return BadRequest(responseResultDto.Data);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Verifier Code Error >>> Email:{inputData.Email} VerifierCode:{inputData.VerifierCode}\n{ex}");
                return BadRequest("查詢密碼驗證發生錯誤.");
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

            /// <summary>
            /// Gets or sets VerifierCode
            /// </summary>
            public string VerifierCode { get; set; }
        }
    }
}