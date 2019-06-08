﻿using GoBike.Team.Repository.Models;
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
        /// 刪除車隊所有活動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteEventDataListOfTeam(string teamID);

        /// <summary>
        /// 取得活動資料
        /// </summary>
        /// <param name="eventID">eventID</param>
        /// <returns>EventData</returns>
        Task<EventData> GetEventData(string eventID);

        /// <summary>
        /// 取得車隊活動資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>EventDatas</returns>
        Task<IEnumerable<EventData>> GetEventDataListOfTeam(string teamID);

        /// <summary>
        /// 更新活動資料
        /// </summary>
        /// <param name="eventData">eventData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateEventData(EventData eventData);

        /// <summary>
        /// 取得會員活動資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>EventDatas</returns>
        Task<IEnumerable<EventData>> GetEventDataListOfMember(string member);
    }
}