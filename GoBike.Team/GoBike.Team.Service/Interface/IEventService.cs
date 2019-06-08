using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Interface
{
    /// <summary>
    /// 活動服務
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// 取消加入活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> CancelJoinEvent(TeamCommandDto teamCommand);

        /// <summary>
        /// 建立活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> CreateEvent(TeamCommandDto teamCommand);

        /// <summary>
        /// 刪除活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> DeleteEvent(TeamCommandDto teamCommand);

        /// <summary>
        /// 刪除車隊所有活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> DeleteEventListOfTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 編輯活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> EditEvent(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得會員活動列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(EventInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<EventInfoDto>, string>> GetEventListOfMember(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得車隊活動列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(EventInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<EventInfoDto>, string>> GetEventListOfTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 加入活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> JoinEvent(TeamCommandDto teamCommand);
    }
}