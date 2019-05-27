using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Interface
{
    /// <summary>
    /// 公告服務
    /// </summary>
    public interface IAnnouncementService
    {
        /// <summary>
        /// 刪除公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> DeleteAnnouncement(TeamCommandDto teamCommand);

        /// <summary>
        /// 編輯公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> EditAnnouncement(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得公告列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(AnnouncementInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<AnnouncementInfoDto>, string>> GetAnnouncementList(TeamCommandDto teamCommand);

        /// <summary>
        /// 發佈公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> PublishAnnouncement(TeamCommandDto teamCommand);
    }
}