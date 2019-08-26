using System;

namespace GoBike.API.Service.Models.Team.Data
{
    /// <summary>
    /// 車隊公告資料
    /// </summary>
    public class TeamAnnouncementDto
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
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets SaveDeadline
        /// </summary>
        public DateTime SaveDeadline { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }
    }
}