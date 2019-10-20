using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GoBike.MGT.APP.Controllers
{
    /// <summary>
    /// 測試 API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok("Get API.");
        }
    }
}