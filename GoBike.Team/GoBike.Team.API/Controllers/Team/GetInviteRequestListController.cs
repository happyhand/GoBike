using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 取得邀請請求列表
    /// </summary>
    [Route("api/team/[controller]")]
    [ApiController]
    public class GetInviteRequestListController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetInviteRequestListController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetInviteRequestListController(ILogger<GetInviteRequestListController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberCommand">memberCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberCommandDto memberCommand)
        {
            try
            {
                Tuple<IEnumerable<TeamInfoDto>, string> result = await this.teamService.GetInviteRequestList(memberCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Invite Request List Error >>> MemberID:{memberCommand.MemberID}\n{ex}");
                return BadRequest("取得邀請請求列表發生錯誤.");
            }
        }
    }
}