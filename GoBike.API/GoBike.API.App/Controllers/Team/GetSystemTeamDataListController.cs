using GoBike.API.App.Filters;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Team
{
    /// <summary>
    /// 取得系統車隊資料功能
    /// </summary>
    [Route("api/Team/[controller]/[action]")]
    [ApiController]
    public class GetSystemTeamDataListController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<GetSystemTeamDataListController> logger;

        /// <summary>
        /// teamService
        /// </summary>
        private readonly ITeamService teamService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="teamService">teamService</param>
        public GetSystemTeamDataListController(ILogger<GetSystemTeamDataListController> logger, ITeamService teamService)
        {
            this.logger = logger;
            this.teamService = teamService;
        }

        /// <summary>
        /// 取得系統車隊資料 - 附近車隊
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Nearby(NearbyPostData postData)
        {
            try
            {
                TeamDto teamDto = new TeamDto()
                {
                    CityID = postData.CityID
                };
                ResponseResultDto responseResult = await this.teamService.GetNearbyTeamDataList(teamDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Nearby Team Data List Error >>> CityID:{postData.CityID}\n{ex}");
                return BadRequest("取得附近車隊資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得系統車隊資料 - 新創車隊
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> NewCreation()
        {
            try
            {
                ResponseResultDto responseResult = await this.teamService.GetNewCreationTeamDataList();
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get New Creation Team Data List Error\n{ex}");
                return BadRequest("取得新創車隊資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得系統車隊資料 - 推薦車隊
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Recommendation()
        {
            try
            {
                ResponseResultDto responseResult = await this.teamService.GetRecommendationTeamDataList();
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Recommendation Team Data List Error\n{ex}");
                return BadRequest("取得推薦車隊資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得附近車隊資料列表 Post 資料
        /// </summary>
        public class NearbyPostData
        {
            /// <summary>
            /// Gets or sets CityID
            /// </summary>
            public int CityID { get; set; }
        }
    }
}