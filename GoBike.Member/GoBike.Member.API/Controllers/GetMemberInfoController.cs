using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 取得會員資訊
    /// </summary>
    [ApiController]
    public class GetMemberInfoController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetMemberInfoController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public GetMemberInfoController(ILogger<GetMemberInfoController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST - 取得會員資訊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetMemberInfo(MemberInfoDto memberInfo)
        {
            try
            {
                Tuple<MemberInfoDto, string> result = await this.memberService.GetMemberInfo(memberInfo);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info Error >>> MemberID:{memberInfo.MemberID} Email:{memberInfo.Email} Mobile:{memberInfo.Mobile}\n{ex}");
                return BadRequest("取得會員資訊發生錯誤");
            }
        }

        /// <summary>
        /// POST - 取得會員資訊列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/[controller]/List")]
        public async Task<IActionResult> GetMemberInfoList(IEnumerable<string> memberIDs)
        {
            try
            {
                Tuple<IEnumerable<MemberInfoDto>, string> result = await this.memberService.GetMemberInfoList(memberIDs);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return BadRequest("取得會員資訊列表發生錯誤");
            }
        }
    }
}