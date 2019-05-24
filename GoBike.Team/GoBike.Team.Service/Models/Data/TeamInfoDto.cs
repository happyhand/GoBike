using System;
using System.Collections.Generic;

namespace GoBike.Team.Service.Models.Data
{
    /// <summary>
    /// 車隊資料
    /// </summary>
    public class TeamInfoDto
    {
        /// <summary>
        /// Gets or sets TeamApplyForJoinIDs
        /// </summary>
        public IEnumerable<string> TeamApplyForJoinIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamBlacklistedIDs
        /// </summary>
        public IEnumerable<string> TeamBlacklistedIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamBlacklistIDs
        /// </summary>
        public IEnumerable<string> TeamBlacklistIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamCoverPhoto
        /// </summary>
        public string TeamCoverPhoto { get; set; }

        /// <summary>
        /// Gets or sets TeamCreateDate
        /// </summary>
        public DateTime TeamCreateDate { get; set; }

        /// <summary>
        /// Gets or sets TeamEventIDs
        /// </summary>
        public IEnumerable<string> TeamEventIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamExamineStatus (1:close, 2:open)
        /// </summary>
        public int TeamExamineStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        public string TeamInfo { get; set; }

        /// <summary>
        /// Gets or sets TeamInviteJoinIDs
        /// </summary>
        public IEnumerable<string> TeamInviteJoinIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamLeaderID
        /// </summary>
        public string TeamLeaderID { get; set; }

        /// <summary>
        /// Gets or sets TeamLocation
        /// </summary>
        public string TeamLocation { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets TeamNewsDate
        /// </summary>
        public DateTime TeamNewsDate { get; set; }

        /// <summary>
        /// Gets or sets TeamPhoto
        /// </summary>
        public string TeamPhoto { get; set; }

        /// <summary>
        /// Gets or sets TeamPlayerIDs
        /// </summary>
        public IEnumerable<string> TeamPlayerIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamSaveDeadline
        /// </summary>
        public DateTime TeamSaveDeadline { get; set; }

        /// <summary>
        /// Gets or sets TeamSearchStatus (1:close, 2:open)
        /// </summary>
        public int TeamSearchStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamViceLeaderIDs
        /// </summary>
        public IEnumerable<string> TeamViceLeaderIDs { get; set; }
    }
}