using GoBike.Team.API.Models;
using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 取得我的車隊資訊列表
    /// </summary>
    [Route("api/team/[controller]")]
    [ApiController]
    public class GetMyTeamInfoListController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetMyTeamInfoListController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetMyTeamInfoListController(ILogger<GetMyTeamInfoListController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberInfoDto memberInfo)
        {
            try
            {
                Tuple<IEnumerable<TeamInfoDto>, IEnumerable<TeamInfoDto>, string> result = await this.teamService.GetMyTeamInfoList(memberInfo.MemberID);
                if (string.IsNullOrEmpty(result.Item3))
                {
                    return Ok(new MyTeamInfoDto { LeaderTeamDatas = result.Item1, JoinTeamDatas = result.Item2 });
                }

                return BadRequest(result.Item3);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get My Team Info List Error >>> MemberID:{memberInfo.MemberID}\n{ex}");
                return BadRequest("取得我的車隊資訊列表發生錯誤.");
            }
        }
    }
}