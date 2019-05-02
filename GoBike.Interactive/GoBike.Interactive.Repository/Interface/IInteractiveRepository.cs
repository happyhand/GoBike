using GoBike.Interactive.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.Repository.Interface
{
    /// <summary>
    /// 互動資料庫
    /// </summary>
    public interface IInteractiveRepository
    {
        /// <summary>
        /// 建立互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>bool</returns>
        Task<bool> CreateInteractiveData(InteractiveData interactiveData);

        /// <summary>
        /// 刪除互動資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteInteractiveData(string memberID);

        /// <summary>
        /// 取得互動資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveData</returns>
        Task<InteractiveData> GetInteractiveData(string memberID);

        /// <summary>
        /// 更新黑名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="blacklistIDs">blacklistIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateBlacklist(string memberID, IEnumerable<string> blacklistIDs);

        /// <summary>
        /// 更新好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="friendListIDs">friendListIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateFriendList(string memberID, IEnumerable<string> friendListIDs);

        /// <summary>
        /// 更新互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateInteractiveData(InteractiveData interactiveData);

        /// <summary>
        /// 更新請求名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="requestListIDs">requestListIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateRequestList(string memberID, IEnumerable<string> requestListIDs);
    }
}