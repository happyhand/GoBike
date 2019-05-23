namespace GoBike.API.Service.Models.Member.Command
{
    /// <summary>
    /// 會員搜尋指令資料
    /// </summary>
    public class MemberSearchCommandDto
    {
        /// <summary>
        /// Gets or sets SearchKey
        /// </summary>
        public string SearchKey { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public string TeamID { get; set; }
    }
}