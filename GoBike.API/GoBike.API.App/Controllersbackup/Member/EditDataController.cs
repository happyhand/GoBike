﻿namespace GoBike.API.App.Controllersbackup.Member
{
    ///// <summary>
    ///// 會員編輯
    ///// </summary>
    //[Route("api/Member/[controller]")]
    //[ApiController]
    //public class EditDataController : ApiController
    //{
    //    /// <summary>
    //    /// logger
    //    /// </summary>
    //    private readonly ILogger<EditDataController> logger;

    //    /// <summary>
    //    /// memberService
    //    /// </summary>
    //    private readonly IMemberService memberService;

    //    /// <summary>
    //    /// 建構式
    //    /// </summary>
    //    /// <param name="logger">logger</param>
    //    /// <param name="memberService">memberService</param>
    //    public EditDataController(ILogger<EditDataController> logger, IMemberService memberService)
    //    {
    //        this.logger = logger;
    //        this.memberService = memberService;
    //    }

    //    /// <summary>
    //    /// 會員編輯
    //    /// </summary>
    //    /// <param name="memberInfo">memberInfo</param>
    //    /// <returns>IActionResult</returns>
    //    [HttpPost]
    //    [CheckLoginActionFilter(true)]
    //    public async Task<IActionResult> Post(MemberInfoDto memberInfo)
    //    {
    //        string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
    //        memberInfo.MemberID = memberID;
    //        try
    //        {
    //            ResponseResultDto responseResult = await this.memberService.EditData(memberInfo);
    //            if (responseResult.Ok)
    //            {
    //                return Ok(responseResult.Data);
    //            }

    //            return BadRequest(responseResult.Data);
    //        }
    //        catch (Exception ex)
    //        {
    //            this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
    //            return BadRequest("會員編輯發生錯誤.");
    //        }
    //    }
    //}
}