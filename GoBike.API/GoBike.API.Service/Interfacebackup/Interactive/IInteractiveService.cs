using GoBike.API.Service.Models.Interactive.Command;
using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Interactive
{
    /// <summary>
    /// 互動服務
    /// </summary>
    public interface IInteractiveService
    {
        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddBlacklist(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriend(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriendRequest(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteBlacklist(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteFriend(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteRequestForAddFriend(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetAddFriendRequestList(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetBlacklist(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetFriendList(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectBeFriend(MemberInteractiveCommandDto memberInteractiveCommand);
    }
}