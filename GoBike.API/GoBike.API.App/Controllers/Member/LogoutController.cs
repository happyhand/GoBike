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
    /// 會員登出
    /// </summary>
    [Route("api/Member/[controller]")]
    [ApiController]
    public class LogoutController : ApiController
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
        public LogoutController(ILogger<LoginController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// 會員登出
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
                ResponseResultDto recordSessionIDResult = await this.memberService.DeleteSessionID(memberID, this.HttpContext.Session.Id);
                if (!recordSessionIDResult.Ok)
                {
                    this.logger.LogError($"Repeat Logout. IP:{this.HttpContext.Connection.RemoteIpAddress}");
                }

                this.HttpContext.Session.Clear();
                return Ok("會員已登出.");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Logout Error\n{ex}");
                return BadRequest("會員登出發生錯誤.");
            }
        }
    }
}