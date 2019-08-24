using GoBike.Service.Service.Interface.Member;
using GoBike.Service.Service.Models.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 取得騎乘資料
    /// </summary>
    [Route("api/Member/[controller]")]
    [ApiController]
    public class GetRideDataController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetRideDataController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public GetRideDataController(ILogger<GetRideDataController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(RideDto rideDto)
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
    }
}