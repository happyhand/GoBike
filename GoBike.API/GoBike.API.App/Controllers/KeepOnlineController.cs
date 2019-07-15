using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoBike.API.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeepOnlineController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<KeepOnlineController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public KeepOnlineController(ILogger<KeepOnlineController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 保持在線
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Keep Online");
        }
    }
}