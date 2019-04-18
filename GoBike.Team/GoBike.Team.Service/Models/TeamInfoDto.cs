namespace GoBike.Team.Service.Models
{
    /// <summary>
    /// 車隊資料
    /// </summary>
    public class TeamInfoDto
    {
        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets TeamLocation
        /// </summary>
        public string TeamLocation { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        public string TeamInfo { get; set; }

        /// <summary>
        /// Gets or sets TeamSearchStatus
        /// </summary>
        public string TeamSearchStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamExamineStatus
        /// </summary>
        public string TeamExamineStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamPhoto
        /// </summary>
        public string TeamPhoto { get; set; }

        /// <summary>
        /// Gets or sets TeamCoverPhoto
        /// </summary>
        public string TeamCoverPhoto { get; set; }
    }
}