using AutoMapper;
using GoBike.API.App.Filters;
using GoBike.API.App.Models.Member;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 申請加入車隊
    /// </summary>
    [ApiController]
    public class ApplyForJoinTeamController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ApplyForJoinTeamController> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="teamService">teamService</param>
        public ApplyForJoinTeamController(ILogger<ApplyForJoinTeamController> logger, IMapper mapper, ITeamService teamService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST - 取得申請請求列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/team/[controller]/getRequestList")]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> GetApplyForRequestList(TeamCommandDto teamCommand)
        {
            string memberID = HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                teamCommand.ExaminerID = memberID;
                ResponseResultDto responseResult = await this.teamService.GetApplyForRequestList(teamCommand);
                if (responseResult.Ok)
                {
                    return Ok(this.mapper.Map<IEnumerable<MemberDetailViewDto>>(responseResult.Data));
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Apply For Request List Error >>> TemaID:{teamCommand.TeamID}\n{ex}");
                return BadRequest("取得申請請求列表發生錯誤.");
            }
        }
    }
}