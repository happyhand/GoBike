using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊活動詳細資訊可視資料
    /// </summary>
    public class TeamEventDetailInfoViewDto : TeamEventSimpleInfoViewDto
    {
        /// <summary>
        /// Gets or sets Altitude
        /// </summary>
        public long Altitude { get; set; }

        /// <summary>
        /// Gets or sets EditType
        /// </summary>
        public int EditType { get; set; }

        /// <summary>
        /// Gets or sets JoinMemberIDs
        /// </summary>
        public IEnumerable<TeamMemberInfoViewDto> JoinMemberIDs { get; set; }

        /// <summary>
        /// Gets or sets RoadLines (TODO)
        /// </summary>
        public IEnumerable<dynamic> RoadLines { get; set; }

        /// <summary>
        /// Gets or sets RoadRemarks (TODO)
        /// </summary>
        public IEnumerable<dynamic> RoadRemarks { get; set; }
    }
}