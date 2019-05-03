using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 移除車輛
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RemoveBikeController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<RemoveBikeController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public RemoveBikeController(ILogger<RemoveBikeController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="bikeInfo">bikeInfo</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(BikeInfoDto bikeInfo)
        {
            try
            {
                string result = await this.memberService.RemoveBike(bikeInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("移除車輛成功");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Bike Error >>> BikeID:{bikeInfo.BikeID}\n{ex}");
                return BadRequest("移除車輛發生錯誤.");
            }
        }
    }
}