namespace GoBike.API.App.Controllersbackup.Interactive
{
    ///// <summary>
    ///// 好友請求功能
    ///// </summary>
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    //public class FriendRequestController : ApiController
    //{
    //    /// <summary>
    //    /// interactiveService
    //    /// </summary>
    //    private readonly IInteractiveService interactiveService;

    //    /// <summary>
    //    /// logger
    //    /// </summary>
    //    private readonly ILogger<FriendRequestController> logger;

    //    /// <summary>
    //    /// 建構式
    //    /// </summary>
    //    /// <param name="logger">logger</param>
    //    /// <param name="interactiveService">interactiveService</param>
    //    public FriendRequestController(ILogger<FriendRequestController> logger, IInteractiveService interactiveService)
    //    {
    //        this.logger = logger;
    //        this.interactiveService = interactiveService;
    //    }

    //    /// <summary>
    //    /// 好友請求功能 - 加入好友請求
    //    /// </summary>
    //    /// <param name="memberBaseCommand">memberBaseCommand</param>
    //    /// <returns>IActionResult</returns>
    //    [HttpPost]
    //    [CheckLoginActionFilter(true)]
    //    public async Task<IActionResult> Add(MemberBaseCommandDto memberBaseCommand)
    //    {
    //        string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
    //        try
    //        {
    //            ResponseResultDto responseResult = await this.interactiveService.AddFriendRequest(new MemberInteractiveCommandDto() { InitiatorID = memberID, ReceiverID = memberBaseCommand.MemberID });
    //            if (responseResult.Ok)
    //            {
    //                return Ok(responseResult.Data);
    //            }

    //            return BadRequest(responseResult.Data);
    //        }
    //        catch (Exception ex)
    //        {
    //            this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{memberID} ReceiverID:{memberBaseCommand.MemberID}\n{ex}");
    //            return BadRequest("加入好友請求發生錯誤.");
    //        }
    //    }

    //    /// <summary>
    //    /// 好友請求功能 - 刪除加入好友請求
    //    /// </summary>
    //    /// <param name="memberBaseCommand">memberBaseCommand</param>
    //    /// <returns>IActionResult</returns>
    //    [HttpPost]
    //    [CheckLoginActionFilter(true)]
    //    public async Task<IActionResult> Delete(MemberBaseCommandDto memberBaseCommand)
    //    {
    //        string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
    //        try
    //        {
    //            ResponseResultDto responseResult = await this.interactiveService.DeleteRequestForAddFriend(new MemberInteractiveCommandDto() { InitiatorID = memberID, ReceiverID = memberBaseCommand.MemberID });
    //            if (responseResult.Ok)
    //            {
    //                return Ok(responseResult.Data);
    //            }

    //            return BadRequest(responseResult.Data);
    //        }
    //        catch (Exception ex)
    //        {
    //            this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{memberID} ReceiverID:{memberBaseCommand.MemberID}\n{ex}");
    //            return BadRequest("刪除加入好友請求發生錯誤.");
    //        }
    //    }

    //    /// <summary>
    //    /// 好友請求功能 - 取得加入好友請求名單
    //    /// </summary>
    //    /// <returns>IActionResult</returns>
    //    [HttpGet]
    //    [CheckLoginActionFilter(true)]
    //    public async Task<IActionResult> Get()
    //    {
    //        string memberID = this.HttpContext.Session.GetObject<string>(CommonFlagHelper.CommonFlag.SessionFlag.MemberID);
    //        try
    //        {
    //            ResponseResultDto responseResult = await this.interactiveService.GetAddFriendRequestList(new MemberInteractiveCommandDto() { InitiatorID = memberID });
    //            if (responseResult.Ok)
    //            {
    //                return Ok(responseResult.Data);
    //            }

    //            return BadRequest(responseResult.Data);
    //        }
    //        catch (Exception ex)
    //        {
    //            this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{memberID}\n{ex}");
    //            return BadRequest("取得加入好友請求名單發生錯誤.");
    //        }
    //    }
    //}
}