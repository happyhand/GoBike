using GoBike.Service.Service.Interface.Common;
using GoBike.Service.Service.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Common
{
    /// <summary>
    /// 取得市區資料列表
    /// </summary>
    [Route("api/Common/[controller]")]
    [ApiController]
    public class GetCityDataListController : ControllerBase
    {
        /// <summary>
        /// commonService
        /// </summary>
        private readonly ICommonService commonService;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetCityDataListController> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="commonService">commonService</param>
        public GetCityDataListController(ILogger<GetCityDataListController> logger, ICommonService commonService)
        {
            this.logger = logger;
            this.commonService = commonService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Tuple<IEnumerable<CityDto>, string> result = await this.commonService.GetCityDataList();
                if (string.IsNullOrEmpty(result.Item2))
                {
                    return Ok(result.Item1);
                }

                return BadRequest(result.Item2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get City Data List Error\n{ex}");
                return BadRequest("取得市區資料列表發生錯誤.");
            }
        }
    }
}