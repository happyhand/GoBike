using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊未加入資訊可視資料
    /// </summary>
    public class TeamNoJoinInfoViewDto
    {
        /// <summary>
        /// Gets or sets FrontCoverUrl
        /// </summary>
        public string FrontCoverUrl { get; set; }

        /// <summary>
        /// Gets or sets JoinStatus
        /// </summary>
        public int JoinStatus { get; set; }

        /// <summary>
        /// Gets or sets MemberList
        /// </summary>
        public IEnumerable<TeamMemberInfoViewDto> MemberList { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        public string TeamInfo { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        public string TeamName { get; set; }
    }
}