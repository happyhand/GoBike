using System;

namespace GoBike.Service.Service.Models.Team
{
    /// <summary>
    /// 車隊互動資料
    /// </summary>
    public class TeamInteractiveDto
    {
        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets InteractiveType
        /// </summary>
        public int InteractiveType { get; set; }

        /// <summary>
        /// Gets or sets InviteID
        /// </summary>
        public string InviteID { get; set; }

        /// <summary>
        /// Gets or sets InviteNickname
        /// </summary>
        public string InviteNickname { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets ReviewFlag
        /// </summary>
        public int ReviewFlag { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }
    }
}