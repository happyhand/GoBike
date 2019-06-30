namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 會員簡易資訊可視資料
    /// </summary>
    public class MemberSimpleInfoViewDto
    {
        /// <summary>
        /// Gets or sets LastOnlineTime
        /// </summary>
        public int LastOnlineTime { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }
    }
}