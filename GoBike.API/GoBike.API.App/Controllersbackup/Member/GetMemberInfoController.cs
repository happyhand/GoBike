namespace GoBike.API.App.Controllersbackup.Member
{
    //    /// <summary>

    //    /// 取得會員資訊
    //    /// </summary>
    //    [Route("api/Member/[controller]")]
    //    [ApiController]
    //    public class GetMemberInfoController : ApiController
    //    {
    //        /// <summary>
    //        /// logger
    //        /// </summary>
    //        private readonly ILogger<GetMemberInfoController> logger;

    //        /// <summary>
    //        /// memberService
    //        /// </summary>
    //        private readonly IMemberService memberService;

    //        /// <summary>
    //        /// 建構式
    //        /// </summary>
    //        /// <param name="logger">logger</param>
    //        /// <param name="memberService">memberService</param>
    //        public GetMemberInfoController(ILogger<GetMemberInfoController> logger, IMemberService memberService)
    //        {
    //            this.logger = logger;
    //            this.memberService = memberService;
    //        }

    //        /// <summary>
    //        /// 取得會員本身資訊
    //        /// </summary>
    //        /// <returns>IActionResult</returns>
    //        [HttpGet]
    //        [CheckLoginActionFilter(true)]
    //        public async Task<IActionResult> Get()
    //        {
    //            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
    //            try
    //            {
    //                ResponseResultDto responseResult = await this.memberService.GetMemberInfo(new MemberBaseCommandDto() { MemberID = memberID });
    //                if (responseResult.Ok)
    //                {
    //                    return Ok(responseResult.Data);
    //                }

    //                return BadRequest(responseResult.Data);
    //            }
    //            catch (Exception ex)
    //            {
    //                this.logger.LogError($"Get Member Info Error >>> MemberID:{memberID}\n{ex}");
    //                return BadRequest("取得會員資訊發生錯誤.");
    //            }
    //        }

    //        /// <summary>
    //        /// 取得其他會員資訊
    //        /// </summary>
    //        /// <param name="memberSearchCommand">memberSearchCommand</param>
    //        /// <returns>IActionResult</returns>
    //        [HttpPost]
    //        [CheckLoginActionFilter(true)]
    //        public async Task<IActionResult> Post(MemberSearchCommandDto memberSearchCommand)
    //        {
    //            string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
    //            try
    //            {
    //                ResponseResultDto responseResult = await this.memberService.InquireMemberInfo(memberID, memberSearchCommand);
    //                if (responseResult.Ok)
    //                {
    //                    return Ok(responseResult.Data);
    //                }

    //                return BadRequest(responseResult.Data);
    //            }
    //            catch (Exception ex)
    //            {
    //                this.logger.LogError($"Get Member Info Error >>> MemberID:{memberID} SearchKey:{memberSearchCommand.SearchKey}\n{ex}");
    //                return BadRequest("取得會員資訊發生錯誤.");
    //            }
    //        }
    //    }
}