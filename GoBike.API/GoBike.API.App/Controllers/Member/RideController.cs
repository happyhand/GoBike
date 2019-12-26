using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員騎乘功能
    /// </summary>
    [Route("api/Member/[controller]/[action]")]
    [ApiController]
    public class RideController : ApiController
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
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Add(RidePostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                RideDto rideDto = new RideDto()
                {
                    MemberID = memberID,
                    Altitude = postData.Altitude,
                    CountyID = postData.CountyID,
                    Distance = postData.Distance,
                    Level = postData.Level,
                    Time = postData.Time,
                    Title = postData.Title,
                    PhotoUrl = postData.PhotoUrl,
                    ShareContent = postData.ShareContent.Select(data => new RideContentDto() { Text = data.Text, Url = data.Url }),
                    SharedType = postData.SharedType,
                    Route = postData.Route.Select(data => new RideRouteDto() { Latitude = data.Latitude, Longitude = data.Longitude })
                };
                ResponseResultDto responseResult = await this.memberService.AddRideData(rideDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Ride Data Error >>> MemberID:{memberID} PostData:{JsonConvert.SerializeObject(postData)}\n{ex}");
                return BadRequest("新增騎乘資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得騎乘資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Get(GetRidePostData postData)
        {
            try
            {
                RideDto rideDto = new RideDto()
                {
                    RideID = postData.RideID
                };
                ResponseResultDto responseResult = await this.memberService.GetRideData(rideDto);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Ride Data Error >>> RideID:{postData.RideID}\n{ex}");
                return BadRequest("取得騎乘資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員的騎乘資料列表
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> GetList(RideOfMemberPostData postData)
        {
            try
            {
                ResponseResultDto responseResult = await this.memberService.GetRideDataListOfMember(postData.MemberID);
                if (responseResult.Ok)
                {
                    return Ok(responseResult.Data);
                }

                return BadRequest(responseResult.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Ride Data List Of Member Error >>> MemberID:{postData.MemberID}\n{ex}");
                return BadRequest("取得會員的騎乘資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得騎乘紀錄 Post 資料
        /// </summary>
        public class GetRidePostData
        {
            /// <summary>
            /// Gets or sets RideID
            /// </summary>
            public string RideID { get; set; }
        }

        /// <summary>
        /// 騎乘分享內容 Post 資料
        /// </summary>
        public class RideContentPostData
        {
            /// <summary>
            /// Gets or sets Text
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// Gets or sets Url
            /// </summary>
            public string Url { get; set; }
        }

        /// <summary>
        /// 會員騎乘 Post 資料
        /// </summary>
        public class RideOfMemberPostData
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string MemberID { get; set; }
        }

        /// <summary>
        /// 騎乘 Post 資料
        /// </summary>
        public class RidePostData
        {
            /// <summary>
            /// Gets or sets Altitude
            /// </summary>
            public string Altitude { get; set; }

            /// <summary>
            /// Gets or sets CountyID
            /// </summary>
            public int CountyID { get; set; }

            /// <summary>
            /// Gets or sets Distance
            /// </summary>
            public string Distance { get; set; }

            /// <summary>
            /// Gets or sets Level
            /// </summary>
            public int Level { get; set; }

            /// <summary>
            /// Gets or sets PhotoUrl
            /// </summary>
            public string PhotoUrl { get; set; }

            /// <summary>
            /// Gets or sets Route
            /// </summary>
            public IEnumerable<RideRoutePostData> Route { get; set; }

            /// <summary>
            /// Gets or sets ShareContent
            /// </summary>
            public IEnumerable<RideContentPostData> ShareContent { get; set; }

            /// <summary>
            /// Gets or sets SharedType
            /// </summary>
            public int SharedType { get; set; }

            /// <summary>
            /// Gets or sets Time
            /// </summary>
            public string Time { get; set; }

            /// <summary>
            /// Gets or sets Title
            /// </summary>
            public string Title { get; set; }
        }

        /// <summary>
        /// 騎乘路徑 Post 資料
        /// </summary>
        public class RideRoutePostData
        {
            /// <summary>
            /// Gets or sets Latitude
            /// </summary>
            public string Latitude { get; set; }

            /// <summary>
            /// Gets or sets Longitude
            /// </summary>
            public string Longitude { get; set; }
        }
    }
}