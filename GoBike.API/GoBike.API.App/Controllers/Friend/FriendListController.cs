using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Friend
{
    /// <summary>
    /// 好友名單
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FriendListController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public FriendListController(ILogger<FriendListController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Get()
        {
            string memberID = HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Friend List Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("取得好友名單發生錯誤.");
            }
        }
    }
}