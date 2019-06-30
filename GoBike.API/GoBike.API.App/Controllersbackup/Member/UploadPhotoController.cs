//using GoBike.API.App.Filters;
//using GoBike.API.Core.Applibs;
//using GoBike.API.Core.Resource;
//using GoBike.API.Service.Interface.Member;
//using GoBike.API.Service.Models.Response;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GoBike.API.App.Controllersbackup.Member
//{
//    /// <summary>
//    /// 上傳頭像
//    /// </summary>
//    [Route("api/Member/[controller]")]
//    [ApiController]
//    public class UploadPhotoController : ApiController
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<UploadPhotoController> logger;

//        /// <summary>
//        /// memberService
//        /// </summary>
//        private readonly IMemberService memberService;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="memberService">memberService</param>
//        public UploadPhotoController(ILogger<UploadPhotoController> logger, IMemberService memberService)
//        {
//            this.logger = logger;
//            this.memberService = memberService;
//        }

//        /// <summary>
//        /// 上傳頭像
//        /// </summary>
//        /// <returns>IActionResult</returns>
//        [HttpPost]
//        [CheckLoginActionFilter(true)]
//        public async Task<IActionResult> Post()
//        {
//            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
//            IFormFileCollection files = this.Request.Form.Files;
//            try
//            {
//                ResponseResultDto responseResult = await this.memberService.UploadPhoto(memberID, files.FirstOrDefault());
//                if (responseResult.Ok)
//                {
//                    return O//}k(responseResult.Data);
//                }

//                return BadRequest(responseResult.Data);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Upload Photo Error >>> MemberID:{memberID} Files:{JsonConvert.SerializeObject(files)}\n{ex}");
//                return BadRequest("上傳頭像發生錯誤.");
//            }
//        }
//    }