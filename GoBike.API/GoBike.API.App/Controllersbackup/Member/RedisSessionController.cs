//using GoBike.API.Core.Applibs;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;

//namespace GoBike.API.App.Controllersbackup.Member
//{
//    /// <summary>
//    /// Redis Session 測試
//    /// </summary>
//    [Route("api/Member/[controller]")]
//    [ApiController]
//    public class RedisSessionController : ControllerBase
//    {
//        /// <summary>
//        /// GET
//        /// </summary>
//        /// <returns>string</returns>
//        [HttpGet]
//        public string Get()
//        {
//            string preLoginDate = HttpContext.Session.GetObject<string>("LoginDate");
//            string loginDate = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
//            HttpContext.Session.SetObject("LoginDate", loginDate);
//            return $"Pre Login Date:{preLoginDate}/nLogin Date:{loginDate}";
//        }
//    }
//}