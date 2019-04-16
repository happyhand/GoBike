using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 重設密碼
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ResetPasswordController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public ResetPasswordController(ILogger<ResetPasswordController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberInfoDto memberInfo)
        {
            try
            {
                Tuple<MemberInfoDto, string> searchResult = await this.memberService.GetMemberInfo(memberInfo);
                if (searchResult.Item1 == null)
                {
                    return BadRequest(searchResult.Item2);
                }

                Tuple<MemberInfoDto, string> result = await this.memberService.EditData(new MemberInfoDto() { MemberID = searchResult.Item1.MemberID, Password = memberInfo.Password }, false);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reset Password Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
                return BadRequest("會員重設密碼發生錯誤.");
            }
        }
    }
}