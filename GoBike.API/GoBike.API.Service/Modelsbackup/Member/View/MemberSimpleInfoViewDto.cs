namespace GoBike.API.Service.Modelsbackup.Member.View
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

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        public string Photo { get; set; }
    }
}