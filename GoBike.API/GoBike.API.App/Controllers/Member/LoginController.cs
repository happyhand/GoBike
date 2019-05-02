﻿using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member;
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
        /// <param name="memberBase">memberBase</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]")]
        public async Task<IActionResult> NormalLogin(MemberBaseDto memberBase)
        {
            try
            {
                ResponseResultDto responseResultDto = await this.memberService.Login(memberBase.Email, memberBase.Password);
                return this.ResponseResultHandler(responseResultDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Normal Login Error >>> Email:{memberBase.Email} Password:{memberBase.Password}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// POST - Token 登入
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/member/[controller]/token")]
        public async Task<IActionResult> TokenLogin(MemberBaseDto memberBase)
        {
            try
            {
                ResponseResultDto responseResultDto = await this.memberService.Login(memberBase.Token);
                return this.ResponseResultHandler(responseResultDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Token Login Error >>> Token:{memberBase.Token}\n{ex}");
                return BadRequest("會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 處理回應資料
        /// </summary>
        /// <param name="responseResultDto">responseResultDto</param>
        /// <returns>IActionResult</returns>
        private IActionResult ResponseResultHandler(ResponseResultDto responseResultDto)
        {
            if (responseResultDto.Ok)
            {
                MemberBaseDto memberBase = responseResultDto.Data as MemberBaseDto;
                this.HttpContext.Session.SetObject(CommonFlagHelper.CommonFlag.SessionFlag.MemberID, memberBase.MemberID);
                return Ok(memberBase.Token);
            }

            return BadRequest(responseResultDto.Data);
        }
    }
}