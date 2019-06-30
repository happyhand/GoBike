using System;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊公告資訊可視資料
    /// </summary>
    public class TeamAnnouncementInfoViewDto
    {
        /// <summary>
        /// Gets or sets AnnouncementID
        /// </summary>
        public string AnnouncementID { get; set; }

        /// <summary>
        /// Gets or sets Context
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets LimitDate
        /// </summary>
        public int LimitDate { get; set; }

        /// <summary>
        /// Gets or sets TeamAnnouncementUpdateType
        /// </summary>
        public int TeamAnnouncementUpdateType { get; set; }
    }
}