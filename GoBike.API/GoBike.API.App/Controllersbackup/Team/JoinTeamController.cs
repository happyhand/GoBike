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
//    /// 加入車隊
//    /// </summary>
//    [Route("api/Team/[controller]/[action]")]
//    [ApiController]
//    public class JoinTeamController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<JoinTeamController> logger;

//        /// <summary>
//        /// teamService
//        /// </summary>
//        private readonly ITeamService teamService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="teamService">teamService</param>
//        public JoinTeamController(ILogger<JoinTeamController> logger, ITeamService teamService)
//        {
//            this.logger = logger;
//            this.teamService = teamService;
//        }

//        /// <summary>
//        /// 加入車隊 - 允許加入車隊
//        /// </summary>
//        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> AllowJoin(TeamInteractiveCommandDto teamInteractiveCommand)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.JoinTeam(memberID, teamInteractiveCommand, false);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Allow Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} ExaminerID:{memberID} TargetID:{teamInteractiveCommand.MemberID} \n{ex}");
//                return BadRequest("允許加入車隊發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 加入車隊 - 邀請加入車隊
//        /// </summary>
//        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> InviteJoin(TeamInteractiveCommandDto teamInteractiveCommand)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.JoinTeam(string.Empty, teamInteractiveCommand, true);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Invite Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} TargetID:{memberID} \n{ex}");
//                return BadRequest("邀請加入車隊發生錯誤.");
//            }
//        }
//    }
//}