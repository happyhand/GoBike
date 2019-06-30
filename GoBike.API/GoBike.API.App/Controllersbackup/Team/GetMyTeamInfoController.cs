//using GoBike.API.App.Filters;
//using GoBike.API.Core.Applibs;
//using GoBike.API.Core.Resource;
//using GoBike.API.Service.Interface.Team;
//using GoBike.API.Service.Models.Response;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;

//namespace GoBike.API.App.Controllersbackup.Team
//{
//    /// <summary>
//    /// 取得我的車隊資訊
//    /// </summary>
//    [Route("api/Team/[controller]")]
//    [ApiController]
//    public class GetMyTeamInfoController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<GetMyTeamInfoController> logger;

//        /// <summary>
//        /// teamService
//        /// </summary>
//        private readonly ITeamService teamService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="teamService">teamService</param>
//        public GetMyTeamInfoController(ILogger<GetMyTeamInfoController> logger, ITeamService teamService)
//        {
//            this.logger = logger;
//            this.teamService = teamService;
//        }

//        /// <summary>
//        /// 取得我的車隊資訊
//        /// </summary>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> Get()
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            try
//            {
//                ResponseResultDto responseResult = await this.teamService.GetMyTeamInfo(memberID);
//                if (responseResult.Ok)
//                {
//                    return Ok(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get My Team Info Error >>> MemberID:{memberID}\n{ex}");
//                return BadRequest("取得我的車隊資訊列表發生錯誤.");
//            }
//        }
//    }
//}