﻿using System;
using System.Collections.Generic;

namespace GoBike.Service.Repository.Models.Team
{
    /// <summary>
    /// 車隊資料
    /// </summary>
    public class TeamDto
    {
        #region Register Data

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        #endregion Register Data

        #region Info Data

        /// Gets or sets CityID
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// Gets or sets ExamineStatus
        /// </summary>
        public int ExamineStatus { get; set; }

        /// <summary>
        /// Gets or sets FrontCoverUrl
        /// </summary>
        public string FrontCoverUrl { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets SearchStatus
        /// </summary>
        public int SearchStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        public string TeamInfo { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        public string TeamName { get; set; }

        #endregion Info Data

        #region Management Data

        /// <summary>
        /// Gets or sets TeamApplyForJoinIDs
        /// </summary>
        public IEnumerable<string> TeamApplyForJoinIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamInviteJoinIDs
        /// </summary>
        public IEnumerable<string> TeamInviteJoinIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamLeaderID
        /// </summary>
        public string TeamLeaderID { get; set; }

        /// <summary>
        /// Gets or sets TeamMemberIDs
        /// </summary>
        public IEnumerable<string> TeamMemberIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamViceLeaderIDs
        /// </summary>
        public IEnumerable<string> TeamViceLeaderIDs { get; set; }

        #endregion Management Data

        #region Extra Data

        /// <summary>
        /// Gets or sets ExecutorID
        /// </summary>
        public string ExecutorID { get; set; }

        /// <summary>
        /// Gets or sets SearchKey
        /// </summary>
        public string SearchKey { get; set; }

        #endregion Extra Data
    }
}