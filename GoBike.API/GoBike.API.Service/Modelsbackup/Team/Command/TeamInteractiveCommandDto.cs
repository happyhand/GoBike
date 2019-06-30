using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.Command
{
    /// <summary>
    /// 車隊互動指令資料
    /// </summary>
    public class TeamInteractiveCommandDto
    {
        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets MemberList
        /// </summary>
        public IEnumerable<string> MemberList { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }
    }
}