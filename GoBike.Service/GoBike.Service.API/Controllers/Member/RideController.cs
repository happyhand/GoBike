using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoBike.Service.Service.Interface.Member;
using GoBike.Service.Service.Models.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 會員騎乘
    /// </summary>
    [Route("api/Member/[controller]/[action]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<RideController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public RideController(ILogger<RideController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// 新增騎乘資料
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Add(RideDto rideDto)
        {
            try
            {
                string result = await this.memberService.AddRideData(rideDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("新增騎乘資料成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Ride Data Error >>> Data:{JsonConvert.SerializeObject(rideDto)}\n{ex}");
                return BadRequest("新增騎乘資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得騎乘資料
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Get(RideDto rideDto)
        {
            try
            {
                Tuple<RideDto, string> result = await this.memberService.GetRideData(rideDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Ride Data Error >>> RideID:{rideDto.RideID}\n{ex}");
                return BadRequest("取得騎乘資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員的騎乘資料列表
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> GetList(MemberDto memberDto)
        {
            try
            {
                Tuple<IEnumerable<RideDto>, string> result = await this.memberService.GetRideDataListOfMember(memberDto);
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Ride Data List Of Member Error >>> MemberID:{memberDto.MemberID}\n{ex}");
                return BadRequest("取得會員的騎乘資料列表發生錯誤.");
            }
        }
    }
}