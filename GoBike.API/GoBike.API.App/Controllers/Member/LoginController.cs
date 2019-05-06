using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Command;
using GoBike.API.Service.Models.Member.Data;
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
        /// POST - 一般登入
        /// </summary>
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]")]
        public async Task<IActionResult> NormalLogin(MemberBaseCommandDto memberBaseCommand)
        {
            try
            {
                ResponseResultDto responseResult = await this.memberService.Login(memberBaseCommand.Email, memberBaseCommand.Password);
                return this.ResponseResultHandler(responseResult);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Normal Login Error >>> Email:{memberBaseCommand.Email} Password:{memberBaseCommand.Password}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// POST - Token 登入
        /// </summary>
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]/token")]
        public async Task<IActionResult> TokenLogin(MemberBaseCommandDto memberBaseCommand)
        {
            try
            {
                ResponseResultDto responseResult = await this.memberService.Login(memberBaseCommand.Token);
                return this.ResponseResultHandler(responseResult);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Token Login Error >>> Token:{memberBaseCommand.Token}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 處理回應資料
        /// </summary>
        /// <param name="responseResult">responseResult</param>
        /// <returns>IActionResult</returns>
        private IActionResult ResponseResultHandler(ResponseResultDto responseResult)
        {
            if (responseResult.Ok)
            {
                MemberInfoDto memberInfo = responseResult.Data as MemberInfoDto;
                this.HttpContext.Session.SetObject(CommonFlagHelper.CommonFlag.SessionFlag.MemberID, memberInfo.MemberID);
                return Ok(memberInfo.Token);
            }

            return BadRequest(responseResult.Data);
        }
    }
}