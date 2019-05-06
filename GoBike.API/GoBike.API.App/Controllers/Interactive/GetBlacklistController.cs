﻿using AutoMapper;
using GoBike.API.App.Filters;
using GoBike.API.App.Models.Member;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Command;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Interactive
{
    /// <summary>
    /// 取得黑名單
    /// </summary>
    [Route("api/Blacklist/[controller]")]
    [ApiController]
    public class GetBlacklistController : ApiController
    {
        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetBlacklistController> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="memberService">memberService</param>
        public GetBlacklistController(ILogger<GetBlacklistController> logger, IMapper mapper, IMemberService memberService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberService = memberService;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Get()
        {
            string memberID = HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.GetBlacklist(new MemberInteractiveCommandDto() { InitiatorID = memberID });
                if (responseResult.Ok)
                {
                    return Ok(this.mapper.Map<IEnumerable<MemberSimpleViewDto>>(responseResult.Data));
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{memberID}\n{ex}");
                return BadRequest("取得黑名單發生錯誤.");
            }
        }
    }
}