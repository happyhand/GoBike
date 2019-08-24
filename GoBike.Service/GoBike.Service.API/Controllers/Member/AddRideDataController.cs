using GoBike.Service.Service.Interface.Member;
using GoBike.Service.Service.Models.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 新增騎乘資料
    /// </summary>
    [Route("api/Member/[controller]")]
    [ApiController]
    public class AddRideDataController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AddRideDataController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public AddRideDataController(ILogger<AddRideDataController> logger, IMemberService memberService)
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
    }
}