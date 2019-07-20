using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 搜尋會員功能
    /// </summary>
    [Route("api/Member/[controller]/[action]")]
    [ApiController]
    public class SearchMemberController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SearchMemberController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public SearchMemberController(ILogger<SearchMemberController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// 搜尋會員 - 搜尋其他會員資料
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Other(SearchMemberPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.SearchMember(postData.SearchKey, memberID);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Other Member Error >>> SearchKey:{postData.SearchKey}\n{ex}");
                return BadRequest("搜尋會員發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋會員 - 取得會員本身資料
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Own()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.SearchMember(memberID, string.Empty);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Own Member Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("搜尋會員發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋會員 - 取得會員設定資料
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Setting()
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                ResponseResultDto responseResult = await this.memberService.GetSettingData(memberID);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Member Setting Error >>> MemberID:{memberID}\n{ex}");
                return BadRequest("取得會員設定資料發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋會員 Post 資料
        /// </summary>
        public class SearchMemberPostData
        {
            /// <summary>
            /// Gets or sets SearchKey
            /// </summary>
            public string SearchKey { get; set; }
        }
    }
}