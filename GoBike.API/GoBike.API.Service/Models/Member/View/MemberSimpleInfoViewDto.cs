namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 會員簡易資訊可視資料
    /// </summary>
    public class MemberSimpleInfoViewDto
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
    }
}