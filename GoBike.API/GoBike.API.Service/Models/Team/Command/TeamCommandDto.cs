using GoBike.API.Service.Models.Team.Command.Data;
using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.Command
{
    /// <summary>
    /// 車隊指令資料
    /// </summary>
    public class TeamCommandDto
    {
        /// <summary>
        /// Gets or sets AnnouncementInfo
        /// </summary>
        public AnnouncementInfoDto AnnouncementInfo { get; set; }

        /// <summary>
        /// Gets or sets ExaminerID
        /// </summary>
        public string ExaminerID { get; set; }

        /// <summary>
        /// Gets or sets TargetID
        /// </summary>
        public string TargetID { get; set; }

        /// <summary>
        /// Gets or sets TargetID
        /// </summary>
        public IEnumerable<string> TargetIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        public TeamInfoDto TeamInfo { get; set; }
    }
}