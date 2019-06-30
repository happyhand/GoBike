//using GoBike.API.App.Filters;
//using GoBike.API.Core.Applibs;
//using GoBike.API.Core.Resource;
//using GoBike.API.Service.Interface.Team;
//using GoBike.API.Service.Models.Response;
//using GoBike.API.Service.Models.Team.Command;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;

//namespace GoBike.API.App.Controllersbackup.Team
//{
//    /// <summary>
//    /// 取得車隊明細資訊
//    /// </summary>
//    [Route("api/Team/[controller]")]
//    [ApiController]
//    public class GetTeamDetailInfoController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<GetTeamDetailInfoController> logger;

//        /// <summary>
//        /// teamService
//        /// </summary>
//        private readonly ITeamService teamService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="teamService">teamService</param>
//        public GetTeamDetailInfoController(ILogger<GetTeamDetailInfoController> logger, ITeamService teamService)
//        {
//            this.logger = logger;
//            this.teamService = teamService;
//        }

//        /// <summary>
//        /// 取得車隊明細資訊
//        /// </summary>
//        /// <param name="teamCommand">teamCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> Post(TeamCommandDto teamCommand)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                teamCommand.TargetID = memberID;
//                ResponseResultDto responseResult = await this.teamService.GetTeamDetailInfo(teamCommand);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get Apply For Request List Error >>> TemaID:{teamCommand.TeamID} MemberID:{memberID}\n{ex}");
//                return BadRequest("取得車隊明細資訊發生錯誤.");
//            }
//        }
//    }
//}