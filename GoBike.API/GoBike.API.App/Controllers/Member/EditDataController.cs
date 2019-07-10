using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員編輯功能
    /// </summary>
    [Route("api/Member/[controller]/[action]")]
    [ApiController]
    public class EditDataController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditDataController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public EditDataController(ILogger<EditDataController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// 會員編輯 - 會員個人資訊
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Info(EditInfoPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            MemberDto memberDto = new MemberDto()
            {
                MemberID = memberID,
                Birthday = postData.Birthday,
                BodyHeight = postData.BodyHeight,
                BodyWeight = postData.BodyWeight,
                Gender = postData.Gender,
                FrontCoverUrl = postData.FrontCoverUrl,
                PhotoUrl = postData.PhotoUrl
            };
            try
            {
                ResponseResultDto responseResult = await this.memberService.EditData(memberDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data For Info Error >>> Data:{JsonConvert.SerializeObject(memberDto)}\n{ex}");
                return BadRequest("會員編輯發生錯誤.");
            }
        }

        /// <summary>
        /// 會員編輯 - 修改密碼
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Password(EditPasswordPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            MemberDto memberDto = new MemberDto()
            {
                MemberID = memberID,
                Password = postData.Password
            };
            try
            {
                ResponseResultDto responseResult = await this.memberService.EditData(memberDto);
                if (responseResult.Ok)
                {
                    return Ok("修改密碼成功.");
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data For Password Error >>> Password:{postData.Password}\n{ex}");
                return BadRequest("會員編輯發生錯誤.");
            }
        }

        /// <summary>
        /// 編輯會員個人資訊 Post 資料
        /// </summary>
        public class EditInfoPostData
        {
            /// <summary>
            /// Gets or sets Birthday
            /// </summary>
            public string Birthday { get; set; }

            /// <summary>
            /// Gets or sets BodyHeight
            /// </summary>
            public double BodyHeight { get; set; }

            /// <summary>
            /// Gets or sets BodyWeight
            /// </summary>
            public double BodyWeight { get; set; }

            /// <summary>
            /// Gets or sets FrontCoverUrl
            /// </summary>
            public string FrontCoverUrl { get; set; }

            /// <summary>
            /// Gets or sets Gender
            /// </summary>
            public int Gender { get; set; }

            /// <summary>
            /// Gets or sets PhotoUrl
            /// </summary>
            public string PhotoUrl { get; set; }
        }

        /// <summary>
        /// 編輯會員密碼 Post 資料
        /// </summary>
        public class EditPasswordPostData
        {
            /// <summary>
            /// Gets or sets Password
            /// </summary>
            public string Password { get; set; }
        }
    }
}