using System;
using System.Collections.Generic;

namespace GoBike.Team.Service.Models.Data
{
    /// <summary>
    /// 活動簡易資料
    /// </summary>
    public class EventSimpleInfoDto
    {
        /// <summary>
        /// Gets or sets CreatorID
        /// </summary>
        public string CreatorID { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets EventDate
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets EventID
        /// </summary>
        public string EventID { get; set; }

        /// <summary>
        /// Gets or sets EventTitle
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        /// Gets or sets HaveSeenMemberIDs
        /// </summary>
        public IEnumerable<string> HaveSeenMemberIDs { get; set; }

        /// <summary>
        /// Gets or sets JoinMemberList
        /// </summary>
        public IEnumerable<string> JoinMemberList { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets TeamPhoto
        /// </summary>
        public string TeamPhoto { get; set; }
    }
}