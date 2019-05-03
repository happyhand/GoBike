using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
    /// <summary>
    /// 新增車輛
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddBikeController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AddBikeController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public AddBikeController(ILogger<AddBikeController> logger, IMemberService memberService)
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
                string result = await this.memberService.AddBike(bikeInfo);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("新增車輛成功");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Bike Error >>> Data:{JsonConvert.SerializeObject(bikeInfo)}\n{ex}");
                return BadRequest("新增車輛發生錯誤.");
            }
        }
    }
}