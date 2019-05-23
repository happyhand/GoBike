using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Interactive.API.Controllers.Member
{
    /// <summary>
    /// 取得會員互動狀態
    /// </summary>
    [Route("api/Member/[controller]")]
    [ApiController]
    public class GetMemberInteractiveStatusController : ControllerBase
    {
        /// <summary>
        /// interactiveService
        /// </summary>
        private readonly IInteractiveService interactiveService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetMemberInteractiveStatusController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveService">interactiveService</param>
        public GetMemberInteractiveStatusController(ILogger<GetMemberInteractiveStatusController> logger, IInteractiveService interactiveService)
        {
            this.logger = logger;
            this.interactiveService = interactiveService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                Tuple<int, string> result = await this.interactiveService.GetMemberInteractiveStatus(interactiveCommand);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Interactive Status Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return BadRequest("取得會員互動狀態發生錯誤.");
            }
        }
    }
}