using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 保持在線
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KeepOnlineController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<KeepOnlineController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public KeepOnlineController(ILogger<KeepOnlineController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// 保持在線
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            ResponseResultDto recordSessionIDResult = await this.memberService.ExtendSessionIDExpire(memberID, this.HttpContext.Session.Id);
            if (!recordSessionIDResult.Ok)
            {
                return BadRequest("Keep Online Fail.");
            }
            return Ok("Keep Online");
        }
    }
}