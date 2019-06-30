//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;

//namespace GoBike.API.App.Controllersbackup.Member
//{
//    /// <summary>
//    /// 會員登出
//    /// </summary>
//    [Route("api/Member/[controller]")]
//    [ApiController]
//    public class LogoutController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<LoginController> logger;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        public LogoutController(ILogger<LoginController> logger)
//        {
//            this.logger = logger;
//        }

//        /// <summary>
//        /// 會員登出
//        /// </summary>
//        /// <returns>IActionResult</returns>
//        [HttpGet]
//        public IActionResult Get()
//        {
//            try
//            {
//                this.HttpContext.Session.Clear();
//                return Ok("會員已登出.");
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Logout Error\n{ex}");
//                return BadRequest("會員登出發生錯誤.");
//            }
//        }
//    }
//}