using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 取得我的車輛資訊列表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GetMyBikeInfoListController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetMyBikeInfoListController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public GetMyBikeInfoListController(ILogger<GetMyBikeInfoListController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberInfoDto memberInfo)
        {
            try
            {
                Tuple<IEnumerable<BikeInfoDto>, string> result = await this.memberService.GetMyBikeInfoList(memberInfo);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get My Bike Info List Error >>> MemberID:{memberInfo.MemberID}\n{ex}");
                return BadRequest("取得我的車輛資訊列表發生錯誤.");
            }
        }
    }
}