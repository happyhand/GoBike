using GoBike.Team.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Repository.Interface
{
    /// <summary>
    /// 活動資料庫
    /// </summary>
    public interface IEventRepository
    {
        /// <summary>
        /// 建立活動資料
        /// </summary>
        /// <param name="eventData">eventData</param>
        /// <returns>bool</returns>
        Task<bool> CreateEventData(EventData eventData);

        /// <summary>
        /// 刪除活動資料
        /// </summary>
        /// <param name="eventID">eventID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteEventData(string eventID);

        /// <summary>
        /// 取得活動資料
        /// </summary>
        /// <param name="eventID">eventID</param>
        /// <returns>EventData</returns>
        Task<EventData> GetEventData(string eventID);

        /// <summary>
        /// 取得活動列表資料
        /// </summary>
        /// <param name="eventIDs">eventIDs</param>
        /// <returns>EventDatas</returns>
        Task<IEnumerable<EventData>> GetEventDataList(IEnumerable<string> eventIDs);

        /// <summary>
        /// 更新活動資料
        /// </summary>
        /// <param name="eventData">eventData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateEventData(EventData eventData);
    }
}