using GoBike.Interactive.Service.Models.Command;
using GoBike.Interactive.Service.Models.Data;
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        Task<string> AddBlacklist(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        Task<string> AddFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        Task<string> AddFriendRequest(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        Task<string> DeleteBlacklist(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        Task<string> DeleteFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        Task<string> DeleteRequestForAddFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        Task<Tuple<IEnumerable<string>, string>> GetAddFriendRequestList(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        Task<Tuple<IEnumerable<string>, string>> GetBlacklist(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        Task<Tuple<IEnumerable<string>, string>> GetFriendList(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        Task<string> RejectBeFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(InteractiveInfoDto, string)</returns>
        Task<Tuple<InteractiveInfoDto, string>> SearchFriend(InteractiveCommandDto interactiveCommand);
    }
}