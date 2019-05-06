﻿using AutoMapper;
using GoBike.API.App.Filters;
using GoBike.API.App.Models.Member;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員編輯
    /// </summary>
    [Route("api/member/[controller]")]
    [ApiController]
    public class EditDataController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditDataController> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="memberService">memberService</param>
        public EditDataController(ILogger<EditDataController> logger, IMapper mapper, IMemberService memberService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(MemberInfoDto memberInfo)
        {
            string memberID = HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            memberInfo.MemberID = memberID;
            try
            {
                ResponseResultDto responseResult = await this.memberService.EditData(memberInfo);
                if (responseResult.Ok)
                {
                    return Ok(this.mapper.Map<MemberDetailViewDto>(responseResult.Data));
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> EditData:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
                return BadRequest("會員編輯發生錯誤.");
            }
        }
    }
}