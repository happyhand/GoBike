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
//    /// 更新車隊隊長
//    /// </summary>
//    [Route("api/Team/[controller]")]
//    [ApiController]
//    public class UpdateTeamLeaderController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<UpdateTeamLeaderController> logger;

//        /// <summary>
//        /// teamService
//        /// </summary>
//        private readonly ITeamService teamService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="teamService">teamService</param>
//        public UpdateTeamLeaderController(ILogger<UpdateTeamLeaderController> logger, ITeamService teamService)
//        {
//            this.logger = logger;
//            this.teamService = teamService;
//        }

//        /// <summary>
//        /// 更新車隊隊長
//        /// </summary>
//        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> Post(TeamInteractiveCommandDto teamInteractiveCommand)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.UpdateTeamLeader(memberID, teamInteractiveCommand);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Update Team Leader Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{memberID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
//                return BadRequest("更新車隊隊長發生錯誤.");
//            }
//        }
//    }
//}