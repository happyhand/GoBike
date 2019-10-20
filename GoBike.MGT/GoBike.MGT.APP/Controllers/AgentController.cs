using GoBike.MGT.Repository.Models.Data;
using GoBike.MGT.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.MGT.APP.Controllers
{
    /// <summary>
    /// 代理商功能
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AgentController> logger;

        /// <summary>
        /// mgtService
        /// </summary>
        private readonly IMgtService mgtService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mgtService">mgtService</param>
        public AgentController(ILogger<AgentController> logger, IMgtService mgtService)
        {
            this.logger = logger;
            this.mgtService = mgtService;
        }

        /// <summary>
        /// 代理商 - 新增代理商資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult Add(AddAgentPostData postData)
        {
            try
            {
                this.mgtService.AddAgent(postData.Nickname, postData.Password);
                return Ok("新增代理商資料成功.");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Agent Error >>> Nickname:{postData.Nickname} Password:{postData.Password}\n{ex}");
                return BadRequest("新增代理商資料發生錯誤.");
            }
        }

        /// <summary>
        /// 代理商 - 取得代理商資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(GetAgentPostData postData)
        {
            try
            {
                Tuple<AgentData, string> result = await this.mgtService.GetAgent(postData.Id);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Agent Error >>> ID:{postData.Id}\n{ex}");
                return BadRequest("取得代理商資料發生錯誤.");
            }
        }

        /// <summary>
        /// 新增代理商資料 Post Data
        /// </summary>
        public class AddAgentPostData
        {
            /// <summary>
            /// Gets or sets Nickname
            /// </summary>
            public string Nickname { get; set; }

            /// <summary>
            /// Gets or sets Password
            /// </summary>
            public string Password { get; set; }
        }

        /// <summary>
        /// 取得代理商資料 Post Data
        /// </summary>
        public class GetAgentPostData
        {
            /// <summary>
            /// Gets or sets Id
            /// </summary>
            public long Id { get; set; }
        }
    }
}