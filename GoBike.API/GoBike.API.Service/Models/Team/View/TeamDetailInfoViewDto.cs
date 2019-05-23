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
        /// Gets or sets TeamActionSetting (0:無設定，1:歷史公告，2:邀請好友，4:發起活動，8:編輯資料，16:發起公告)
        /// </summary>
        public int TeamActionSetting { get; set; }

        /// <summary>
        /// Gets or sets MemberList
        /// </summary>
        public IEnumerable<TeamMemberInfoViewDto> TeamMemberList { get; set; }

        /// <summary>
        /// Gets or sets ApplyForRequestList
        /// </summary>
        public IEnumerable<MemberSimpleInfoViewDto> ApplyForRequestList { get; set; }

        #region TODO

        /// <summary>
        /// Gets or sets newAnnouncement
        /// </summary>
        public dynamic newAnnouncement { get; set; }

        /// <summary>
        /// Gets or sets EventList
        /// </summary>
        public IEnumerable<dynamic> EventList { get; set; }

        #endregion TODO
    }
}