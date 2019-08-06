using GoBike.API.App.Filters;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 新增騎乘資料
    /// </summary>
    [Route("api/Member/[controller]")]
    [ApiController]
    public class AddRideDataController : ApiController
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
        /// 新增騎乘資料
        /// </summary>
        /// <param name="postData">postData</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<IActionResult> Post(RideDataPostData postData)
        {
            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
            try
            {
                RideDto rideDto = new RideDto()
                {
                    MemberID = memberID,
                    Climb = postData.Climb,
                    Content = postData.Content,
                    CityID = postData.CityID,
                    Distance = postData.Distance,
                    Level = postData.Level,
                    RideTime = postData.RideTime,
                    Title = postData.Title
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
                this.logger.LogError($"Add Ride Data Error >>> PostData:{JsonConvert.SerializeObject(postData)}\n{ex}");
                return BadRequest("新增騎乘資料發生錯誤.");
            }
        }

        /// <summary>
        /// Post 資料
        /// </summary>
        public class RideDataPostData
        {
            /// <summary>
            /// Gets or sets County
            /// </summary>
            public int CityID { get; set; }

            /// <summary>
            /// Gets or sets Climb
            /// </summary>
            public double Climb { get; set; }

            /// <summary>
            /// Gets or sets Content
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// Gets or sets Distance
            /// </summary>
            public double Distance { get; set; }

            /// <summary>
            /// Gets or sets Level
            /// </summary>
            public int Level { get; set; }

            /// <summary>
            /// Gets or sets RideTime
            /// </summary>
            public long RideTime { get; set; }

            /// <summary>
            /// Gets or sets Title
            /// </summary>
            public string Title { get; set; }
        }
    }
}