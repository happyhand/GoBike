namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 車隊會員資訊可視資料
    /// </summary>
    public class TeamMemberInfoView
    {
        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets OnlineType
        /// </summary>
        public int OnlineType { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets TeamIdentity
        /// </summary>
        public int TeamIdentity { get; set; }
    }
}