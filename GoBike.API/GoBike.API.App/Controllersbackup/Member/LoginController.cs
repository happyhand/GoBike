//using GoBike.API.Service.Interface.Member;
//using GoBike.API.Service.Models.Member.Command;
//using GoBike.API.Service.Models.Response;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;

//namespace GoBike.API.App.Controllersbackup.Member
//{
//    /// <summary>
//    /// 會員登入
//    /// </summary>
//    [ApiController]
//    public class LoginController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<LoginController> logger;

//        /// <summary>
//        /// memberService
//        /// </summary>
//        private readonly IMemberService memberService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="memberService">memberService</param>
//        public LoginController(ILogger<LoginController> logger, IMemberService memberService)
//        {
//            this.logger = logger;
//            this.memberService = memberService;
//        }

//        /// <summary>
//        /// 會員登入 - 一般登入
//        /// </summary>
//        /// <param name="memberBaseCommand">memberBaseCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [Route("api/Member/[controller]")]
//        public async Task<IActionResult> NormalLogin(MemberBaseCommandDto memberBaseCommand)
//        {
//            try
//            {
//                ResponseResultDto responseResult = await this.memberService.Login(this.HttpContext, memberBaseCommand.Email, memberBaseCommand.Password);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Normal Login Error >>> Email:{memberBaseCommand.Email} Password:{memberBaseCommand.Password}\n{ex}");
//                return BadRequest("會員登入發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 會員登入 - Token 登入
//        /// </summary>
//        /// <param name="memberBaseCommand">memberBaseCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [Route("api/Member/[controller]/Token")]
//        public async Task<IActionResult> TokenLogin(MemberBaseCommandDto memberBaseCommand)
//        {
//            try
//            {
//                ResponseResultDto responseResult = await this.memberService.Login(this.HttpContext, memberBaseCommand.Token);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Token Login Error >>> Token:{memberBaseCommand.Token}\n{ex}");
//                return BadRequest("會員登入發生錯誤.");
//            }
//        }
//    }
//}