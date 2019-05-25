using GoBike.API.Service.Models.Member.View;
using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊明細資訊可視資料
    /// </summary>
    public class TeamDetailInfoViewDto : TeamInfoViewDto
    {
        /// <summary>
        /// Gets or sets ApplyForRequestList
        /// </summary>
        public IEnumerable<MemberSimpleInfoViewDto> ApplyForRequestList { get; set; }

        /// <summary>
        /// Gets or sets TeamActionSetting
        /// </summary>
        public int TeamActionSetting { get; set; }

        /// <summary>
        /// Gets or sets MemberList
        /// </summary>
        public IEnumerable<TeamMemberInfoViewDto> TeamMemberList { get; set; }

        #region TODO

        /// <summary>
        /// Gets or sets EventList
        /// </summary>
        public IEnumerable<dynamic> EventList { get; set; }

        /// <summary>
        /// Gets or sets NewAnnouncement
        /// </summary>
        public dynamic NewAnnouncement { get; set; }

        #endregion TODO
    }
}