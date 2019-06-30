using GoBike.Service.Repository.Models.Member;
using GoBike.Service.Service.Interface.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 會員登入
    /// </summary>
    [ApiController]
    public class LoginController : ControllerBase
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
        /// FB 登入
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> FB(MemberDto memberDto)
        {
            try
            {
                Tuple<string, string> result = await this.memberService.LoginFB(memberDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login FB Error >>> FBToken:{memberDto.FBToken}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// Google 登入
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/[action]")]
        public async Task<IActionResult> Google(MemberDto memberDto)
        {
            try
            {
                Tuple<string, string> result = await this.memberService.LoginGoogle(memberDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Google Error >>> GoogleToken:{memberDto.GoogleToken}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 一般登入
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]")]
        public async Task<IActionResult> Normal(MemberDto memberDto)
        {
            try
            {
                Tuple<string, string> result = await this.memberService.Login(memberDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Error >>> Email:{memberDto.Email} Password:{memberDto.Password}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }
    }
}