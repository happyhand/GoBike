using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.API.Controllers.Team
{
    /// <summary>
    /// 取得會員的車隊資訊列表
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class GetTeamInfoListOfMemberController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetTeamInfoListOfMemberController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetTeamInfoListOfMemberController(ILogger<GetTeamInfoListOfMemberController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(TeamCommandDto teamCommand)
        {
            try
            {
                Tuple<dynamic[], string> result = await this.teamService.GetTeamInfoListOfMember(teamCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Info List Of Member Error >>> MemberID:{teamCommand.TargetID}\n{ex}");
                return BadRequest("取得會員的車隊資訊列表發生錯誤.");
            }
        }
    }
}