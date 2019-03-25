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
    /// 取得會員資訊
    /// </summary>
    [Route("api/member/[controller]")]
    [ApiController]
    public class GetMemberInfoController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

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
        public GetMemberInfoController(ILogger<GetMemberInfoController> logger, IMapper mapper, IMemberService memberService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberService = memberService;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns>ResultModel</returns>
        [HttpGet]
        [CheckLoginActionFilter(true)]
        public async Task<ResultModel> Get()
        {
            string memberID = HttpContext.Session.GetObject<string>(Utility.Session_MemberID);
            GetMemberInfoRespone result = await this.memberService.GetMemberInfo(memberID);
            return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage, ResultData = this.mapper.Map<MemberInfo>(result.MemberData) };
        }
    }
}