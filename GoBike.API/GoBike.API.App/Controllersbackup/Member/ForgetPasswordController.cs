namespace GoBike.API.App.Controllersbackup.Member
{
    ///// <summary>
    ///// 忘記密碼
    ///// </summary>
    //[ApiController]
    //public class ForgetPasswordController : ControllerBase
    //{
    //    /// <summary>
    //    /// logger
    //    /// </summary>
    //    private readonly ILogger<ForgetPasswordController> logger;

    //    /// <summary>
    //    /// memberService
    //    /// </summary>
    //    private readonly IMemberService memberService;

    //    /// <summary>
    //    /// verifierService
    //    /// </summary>
    //    private readonly IVerifierService verifierService;

    //    /// <summary>
    //    /// 建構式
    //    /// </summary>
    //    /// <param name="logger">logger</param>
    //    /// <param name="memberService">memberService</param>
    //    /// <param name="verifierService">verifierService</param>
    //    public ForgetPasswordController(ILogger<ForgetPasswordController> logger, IMemberService memberService, IVerifierService verifierService)
    //    {
    //        this.logger = logger;
    //        this.memberService = memberService;
    //        this.verifierService = verifierService;
    //    }

    //    /// <summary>
    //    /// 忘記密碼 - 請求產生驗證碼
    //    /// </summary>
    //    /// <param name="verifierCommand">verifierCommand</param>
    //    /// <returns>IActionResult</returns>
    //    [HttpPost]
    //    [Route("api/Member/[controller]")]
    //    public async Task<IActionResult> GetVerifierCode(VerifierCommandDto verifierCommand)
    //    {
    //        try
    //        {
    //            verifierCommand.Type = "Password";
    //            verifierCommand.VerifierCode = await this.verifierService.GetVerifierCode(verifierCommand);
    //            EmailContext emailContext = EmailContext.GetVerifierCodetEmailContext(verifierCommand.Email, verifierCommand.VerifierCode);
    //            ResponseResultDto responseResult = await this.verifierService.SendVerifierCode(verifierCommand, emailContext);
    //            if (responseResult.Ok)
    //            {
    //                return Ok(responseResult.Data);
    //            }

    //            return BadRequest(responseResult.Data);
    //        }
    //        catch (Exception ex)
    //        {
    //            this.logger.LogError($"Get Verifier Code Error >>> Data:{JsonConvert.SerializeObject(verifierCommand)}\n{ex}");
    //            return BadRequest("取得查詢密碼驗證碼發生錯誤.");
    //        }
    //    }

    //    /// <summary>
    //    /// 忘記密碼 - 驗證驗證碼並重設密碼
    //    /// </summary>
    //    /// <param name="verifierCommand">verifierCommand</param>
    //    /// <returns>IActionResult</returns>
    //    [HttpPost]
    //    [Route("api/Member/[controller]/VerifierCode")]
    //    public async Task<IActionResult> ResetPassword(VerifierCommandDto verifierCommand)
    //    {
    //        try
    //        {
    //            verifierCommand.Type = "Password";
    //            bool isValidVerifierCode = await this.verifierService.IsValidVerifierCode(verifierCommand);
    //            if (!isValidVerifierCode)
    //            {
    //                return BadRequest("驗證碼驗證失敗.");
    //            }
    //            else
    //            {
    //                ResponseResultDto responseResult = await this.memberService.ResetPassword(new MemberBaseCommandDto() { Email = verifierCommand.Email });
    //                if (responseResult.Ok)
    //                {
    //                    return Ok(responseResult.Data);
    //                }

    //                return BadRequest(responseResult.Data);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            this.logger.LogError($"Reset Password Error >>> Email:{verifierCommand.Email} VerifierCode:{verifierCommand.VerifierCode}\n{ex}");
    //            return BadRequest("重設密碼發生錯誤.");
    //        }
    //    }
    //}
}