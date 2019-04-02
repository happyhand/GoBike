using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員登出
    /// </summary>
    [Route("api/member/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public LogoutController(ILogger<LoginController> logger)
        {
            this.logger = logger;
        }

        ///// <summary>
        ///// POST
        ///// </summary>
        ///// <returns>ResultModel</returns>
        //[HttpPost]
        //public ResultModel Post()
        //{
        //    try
        //    {
        //        HttpContext.Session.Clear();
        //        return new ResultModel() { ResultCode = 1, ResultMessage = "Logout Success" };
        //    }
        //    catch (Exception ex)
        //    {
        //        this.logger.LogError($"Logout Error\n{ex}");
        //        return new ResultModel() { ResultCode = -999, ResultMessage = "Logout Error" };
        //    }
        //}
    }
}