using GoBike.API.App.Filters;
using GoBike.API.Service.Interface.Common;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Common
{
    /// <summary>
    /// 取得市區資料列表
    /// </summary>
    [Route("api/Common/[controller]")]
    [ApiController]
    public class GetCityDataListController : ApiController
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
        /// 取得市區資料列表
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Get()
        {
            try
            {
                ResponseResultDto responseResult = await this.commonService.GetCityDataList();
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get City Data List Error\n{ex}");
                return BadRequest("取得市區資料列表發生錯誤.");
            }
        }
    }
}