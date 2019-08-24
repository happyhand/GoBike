using GoBike.Service.Service.Interface.Member;
using GoBike.Service.Service.Models.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 搜尋會員
    /// </summary>
    [ApiController]
    public class SearchMemberController : ControllerBase
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
        /// 搜尋會員列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]/List")]
        public async Task<IActionResult> SearchList(IEnumerable<string> memberIDs)
        {
            try
            {
                Tuple<IEnumerable<MemberDto>, string> result = await this.memberService.SearchMemberList(memberIDs);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Member List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return BadRequest("搜尋會員列表發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋單一會員
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("api/Member/[controller]")]
        public async Task<IActionResult> SearchSingle(MemberDto memberDto)
        {
            try
            {
                Tuple<MemberDto, string> result = await this.memberService.SearchMember(memberDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Member Error >>> MemberID:{memberDto.MemberID} Email:{memberDto.Email}\n{ex}");
                return BadRequest("搜尋會員發生錯誤.");
            }
        }
    }
}