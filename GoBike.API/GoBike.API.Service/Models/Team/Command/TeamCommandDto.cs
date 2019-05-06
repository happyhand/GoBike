﻿using GoBike.API.Service.Models.Team.Data;

namespace GoBike.API.Service.Models.Team.Command
{
    /// <summary>
    /// 車隊指令資料
    /// </summary>
    public class TeamCommandDto
    {
        /// <summary>
        /// Gets or sets Data
        /// </summary>
        public TeamInfoDto Data { get; set; }

        /// <summary>
        /// Gets or sets ExaminerID
        /// </summary>
        public string ExaminerID { get; set; }

        /// <summary>
        /// Gets or sets TargetID
        /// </summary>
        public string TargetID { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }
    }
}