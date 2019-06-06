using System;

namespace GoBike.API.Service.Models.Team.Command.Data
{
    /// <summary>
    /// 公告資料
    /// </summary>
    public class AnnouncementInfoDto
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
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }
    }
}