using AutoMapper;
using GoBike.API.App.Applibs;
using GoBike.API.App.Applibs.Filters;
using GoBike.API.App.Models.Response;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Response;
using GoBikeAPI.App.Models.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Member
{
    /// <summary>
    /// 會員編輯
    /// </summary>
    [Route("api/member/[controller]")]
    [ApiController]
    public class EditDataController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditDataController> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public EditDataController(ILogger<EditDataController> logger, IMapper mapper, IMemberService memberService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberService = memberService;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <returns>ResultModel</returns>
        [HttpPost]
        [CheckLoginActionFilter(true)]
        public async Task<ResultModel> Post(RequestData requestData)
        {
            string memberID = HttpContext.Session.GetObject<string>(Utility.Session_MemberID);
            EditDataRespone result = await this.memberService.EditData(new EditDataRequest()
            {
                MemberID = memberID,
                BirthDayDate = requestData.BirthDayDate,
                BodyHeight = requestData.BodyHeight,
                BodyWeight = requestData.BodyWeight,
                Gender = requestData.Gender,
                Mobile = requestData.Mobile,
                Nickname = requestData.Nickname
            });
            return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage, ResultData = this.mapper.Map<MemberInfo>(result.MemberData) };
        }

        /// <summary>
        /// 請求參數
        /// </summary>
        public class RequestData
        {
            /// <summary>
            /// Gets or sets BirthDayDate
            /// </summary>
            public string BirthDayDate { get; set; }

            /// <summary>
            /// Gets or sets BodyHeight
            /// </summary>
            public string BodyHeight { get; set; }

            /// <summary>
            /// Gets or sets BodyWeight
            /// </summary>
            public string BodyWeight { get; set; }

            /// <summary>
            /// Gets or sets Gender
            /// </summary>
            public string Gender { get; set; }

            /// <summary>
            /// Gets or sets Mobile
            /// </summary>
            public string Mobile { get; set; }

            /// <summary>
            /// Gets or sets Nickname
            /// </summary>
            public string Nickname { get; set; }
        }
    }
}