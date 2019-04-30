namespace GoBike.Team.Service.Models.Command
{
    /// <summary>
    /// 車隊搜尋指令資料
    /// </summary>
    public class TeamSearchCommandDto
    {
        /// <summary>
        /// Gets or sets SearcherID
        /// </summary>
        public string SearcherID { get; set; }

        /// <summary>
        /// Gets or sets SearchKey
        /// </summary>
        public string SearchKey { get; set; }
    }
}