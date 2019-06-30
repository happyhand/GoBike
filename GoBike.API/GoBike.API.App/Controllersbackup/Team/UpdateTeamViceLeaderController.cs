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
//    /// 更新車隊副隊長
//    /// </summary>
//    [Route("api/Team/[controller]/[action]")]
//    [ApiController]
//    public class UpdateTeamViceLeaderController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<UpdateTeamViceLeaderController> logger;

//        /// <summary>
//        /// teamService
//        /// </summary>
//        private readonly ITeamService teamService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="teamService">teamService</param>
//        public UpdateTeamViceLeaderController(ILogger<UpdateTeamViceLeaderController> logger, ITeamService teamService)
//        {
//            this.logger = logger;
//            this.teamService = teamService;
//        }

//        /// <summary>
//        /// 更新車隊副隊長 - 新增
//        /// </summary>
//        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> Add(TeamInteractiveCommandDto teamInteractiveCommand)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.UpdateTeamViceLeader(memberID, teamInteractiveCommand, true);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Add Team ViceLeader Error >>> TeamID:{teamInteractiveCommand.TeamID} ExaminerID:{memberID} TargetID:{teamInteractiveCommand.MemberID} \n{ex}");
//                return BadRequest("新增車隊副隊長發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 更新車隊副隊長 - 移除
//        /// </summary>
//        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> Remove(TeamInteractiveCommandDto teamInteractiveCommand)
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.UpdateTeamViceLeader(memberID, teamInteractiveCommand, false);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Remove Team ViceLeader Error >>> TeamID:{teamInteractiveCommand.TeamID} ExaminerID:{memberID} TargetID:{teamInteractiveCommand.MemberID} \n{ex}");
//                return BadRequest("移除車隊副隊長發生錯誤.");
//            }
//        }
//    }
//}