﻿using AutoMapper;
using GoBike.API.App.Filters;
using GoBike.API.App.Models.Member;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interactive;
using GoBike.API.Service.Interface.Interactive;
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
    [Route("api/blacklist/[controller]")]
    [ApiController]
    public class GetBlacklistController : ApiController
    {
        /// <summary>
        /// memberService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public GetBlacklistController(ILogger<GetFriendListController> logger, IMapper mapper, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.interactiveService = interactiveService;
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
                ResponseResultDto responseResultDto = await this.interactiveService.GetBlacklist(new InteractiveInfoDto() { InitiatorID = memberID });
                if (responseResultDto.Ok)
                {
                    return Ok(this.mapper.Map<IEnumerable<MemberViewDto>>(responseResultDto.Data));
                }

                return BadRequest(responseResultDto.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("取得黑名單發生錯誤.");
            }
        }
    }
}