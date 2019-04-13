using GoBike.Interactive.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.Service.Interface
{
    /// <summary>
    /// 互動服務
    /// </summary>
    public interface IInteractiveService
    {
        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        Task<string> AddBlacklist(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        Task<string> AddFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        Task<string> AddFriendRequest(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        Task<string> DeleteBlacklist(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        Task<string> DeleteFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        Task<string> DeleteRequestForAddFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetAddFriendRequestList(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetBlacklist(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetFriendList(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        Task<string> RejectBeFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        Task<Tuple<MemberInfoDto, string>> SearchFriend(InteractiveInfoDto interactiveInfo);
    }
}