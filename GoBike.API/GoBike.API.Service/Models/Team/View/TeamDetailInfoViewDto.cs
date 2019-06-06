using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊明細資訊可視資料
    /// </summary>
    public class TeamDetailInfoViewDto : TeamInfoViewDto
    {
        /// <summary>
        /// Gets or sets AnnouncementUpdateType
        /// </summary>
        public int AnnouncementUpdateType { get; set; }

        /// <summary>
        /// Gets or sets ApplyForUpdateType
        /// </summary>
        public int ApplyForUpdateType { get; set; }

        /// <summary>
        /// Gets or sets EventUpdateType
        /// </summary>
        public int EventUpdateType { get; set; }

        /// <summary>
        /// Gets or sets TeamActionSetting
        /// </summary>
        public int TeamActionSetting { get; set; }

        /// <summary>
        /// Gets or sets TeamIdentity
        /// </summary>
        public int TeamIdentity { get; set; }

        /// <summary>
        /// Gets or sets MemberList
        /// </summary>
        public IEnumerable<TeamMemberInfoViewDto> TeamMemberList { get; set; }

        #region TODO

        /// <summary>
        /// Gets or sets EventList
        /// </summary>
        public IEnumerable<dynamic> EventList { get; set; }

        #endregion TODO
    }
}