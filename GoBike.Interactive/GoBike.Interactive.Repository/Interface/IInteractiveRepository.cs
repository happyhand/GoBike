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
        /// <param name="initiatorID">initiatorID</param>
        /// <param name="passiveID">passiveID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteInteractiveData(string initiatorID, string passiveID);

        /// <summary>
        /// 取得加入好友請求列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        Task<IEnumerable<InteractiveData>> GetAddFriendRequestList(string memberID);

        /// <summary>
        /// 取得黑名單列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        Task<IEnumerable<InteractiveData>> GetBlacklist(string memberID);

        /// <summary>
        /// 取得好友列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        Task<IEnumerable<InteractiveData>> GetFriendList(string memberID);

        /// <summary>
        /// 取得指定互動資料
        /// </summary>
        /// <param name="initiatorID">initiatorID</param>
        /// <param name="passiveID">passiveID</param>
        /// <returns>InteractiveData</returns>
        Task<InteractiveData> GetInteractiveData(string initiatorID, string passiveID);

        /// <summary>
        /// 更新互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateInteractiveData(InteractiveData interactiveData);
    }
}