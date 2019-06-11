using System;
using System.Collections.Generic;

namespace GoBike.Team.Service.Models.Data
{
    /// <summary>
    /// 活動資料
    /// </summary>
    public class EventInfoDto
    {
        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

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
        /// Gets or sets HaveSeenMemberIDs
        /// </summary>
        public IEnumerable<string> HaveSeenMemberIDs { get; set; }

        /// <summary>
        /// Gets or sets JoinMemberList
        /// </summary>
        public IEnumerable<string> JoinMemberList { get; set; }

        /// <summary>
        /// Gets or sets RoutePoints
        /// </summary>
        public IEnumerable<string> RoutePoints { get; set; }

        /// <summary>
        /// Gets or sets SaveDeadline
        /// </summary>
        public DateTime SaveDeadline { get; set; }

        /// <summary>
        /// Gets or sets Site
        /// </summary>
        public string Site { get; set; }

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