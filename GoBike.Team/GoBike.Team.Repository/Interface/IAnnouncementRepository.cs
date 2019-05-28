using GoBike.Team.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Repository.Interface
{
    /// <summary>
    /// 公告資料庫
    /// </summary>
    public interface IAnnouncementRepository
    {
        /// <summary>
        /// 建立公告資料
        /// </summary>
        /// <param name="AnnouncementData">AnnouncementData</param>
        /// <returns>bool</returns>
        Task<bool> CreateAnnouncementData(AnnouncementData announcementData);

        /// <summary>
        /// 刪除公告資料
        /// </summary>
        /// <param name="announcementID">announcementID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteAnnouncementData(string announcementID);

        /// <summary>
        /// 刪除車隊所有公告資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteAnnouncementDataListOfTeam(string teamID);

        /// <summary>
        /// 取得公告資料
        /// </summary>
        /// <param name="announcementID">announcementID</param>
        /// <returns>AnnouncementData</returns>
        Task<AnnouncementData> GetAnnouncementData(string announcementID);

        /// <summary>
        /// 取得車隊公告資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>AnnouncementDatas</returns>
        Task<IEnumerable<AnnouncementData>> GetAnnouncementDataListOfTeam(string teamID);

        /// <summary>
        /// 更新公告資料
        /// </summary>
        /// <param name="announcementData">announcementData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateAnnouncementData(AnnouncementData announcementData);
    }
}