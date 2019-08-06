using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 取得會員的車隊列表
    /// </summary>
    [Route("api/Team/[controller]")]
    [ApiController]
    public class GetTeamListOfMemberController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetTeamListOfMemberController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetTeamListOfMemberController(ILogger<GetTeamListOfMemberController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 取得會員的車隊列表
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Get()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    ExecutorID = memberID
                };

                ResponseResultDto responseResult = await this.teamService.GetTeamListOfMember(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team List Of Member Error >>> ExecutorID:{memberID}\n{ex}");
                return BadRequest("取得會員的車隊列表發生錯誤.");
            }
        }
    }
}