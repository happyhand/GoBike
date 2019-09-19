using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊詳細資訊可視資料
    /// </summary>
    public class TeamDetailInfoViewDto : TeamSimpleInfoView
    {
        /// <summary>
        /// Gets or sets AnnouncementList
        /// </summary>
        public IEnumerable<TeamAnnouncementInfoViewDto> AnnouncementList { get; set; }

        /// <summary>
        /// Gets or sets ApplyForList
        /// </summary>
        public IEnumerable<TeamInteractiveInfoViewDto> ApplyForList { get; set; }

        /// <summary>
        /// Gets or sets FrontCoverUrl
        /// </summary>
        public string FrontCoverUrl { get; set; }

        /// <summary>
        /// Gets or sets InviteList
        /// </summary>
        public IEnumerable<TeamInteractiveInfoViewDto> InviteList { get; set; }

        /// <summary>
        /// Gets or sets MemberList
        /// </summary>
        public IEnumerable<TeamMemberInfoView> MemberList { get; set; }

        /// <summary>
        /// Gets or sets TeamIdentity
        /// </summary>
        public int TeamIdentity { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        public string TeamInfo { get; set; }
    }
}