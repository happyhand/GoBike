using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models.Command;
using GoBike.Interactive.Service.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers.Friend
{
    /// <summary>
    /// 搜尋好友
    /// </summary>
    [Route("api/friend/[controller]")]
    [ApiController]
    public class SearchFriendController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SearchFriendController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public SearchFriendController(ILogger<SearchFriendController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                Tuple<InteractiveInfoDto, string> result = await this.interactiveService.SearchFriend(interactiveCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return BadRequest("搜尋好友發生錯誤.");
            }
        }
    }
}