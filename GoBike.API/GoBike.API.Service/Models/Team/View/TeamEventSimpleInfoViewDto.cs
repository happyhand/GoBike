using System;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊活動簡易資訊可視資料
    /// </summary>
    public class TeamEventSimpleInfoViewDto
    {
        /// <summary>
        /// Gets or sets Distance
        /// </summary>
        public long Distance { get; set; }

        /// <summary>
        /// Gets or sets EventDate
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets EventID
        /// </summary>
        public string EventID { get; set; }

        /// <summary>
        /// Gets or sets Executor
        /// </summary>
        public TeamMemberInfoViewDto Executor { get; set; }

        /// <summary>
        /// Gets or sets JoinType
        /// </summary>
        public int JoinType { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public string Title { get; set; }
    }
}