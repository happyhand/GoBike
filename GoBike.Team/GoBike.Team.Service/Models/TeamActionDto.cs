namespace GoBike.Team.Service.Models
{
    /// <summary>
    /// 車隊命令資料
    /// </summary>
    public class TeamActionDto
    {
        /// <summary>
        /// Gets or sets ActionID
        /// </summary>
        public string ActionID { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }
    }
}