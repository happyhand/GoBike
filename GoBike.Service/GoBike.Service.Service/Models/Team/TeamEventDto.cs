﻿using System;
using System.Collections.Generic;

namespace GoBike.Service.Service.Models.Team
{
    /// <summary>
    /// 車隊活動資料
    /// </summary>
    public class TeamEventDto
    {
        /// <summary>
        /// Gets or sets Altitude
        /// </summary>
        public long Altitude { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

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
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets RoadLines (TODO)
        /// </summary>
        public IEnumerable<dynamic> RoadLines { get; set; }

        /// <summary>
        /// Gets or sets RoadRemarks (TODO)
        /// </summary>
        public IEnumerable<dynamic> RoadRemarks { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        #region Extra Data

        /// <summary>
        /// Gets or sets EditType
        /// </summary>
        public int EditType { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        #endregion Extra Data
    }
}