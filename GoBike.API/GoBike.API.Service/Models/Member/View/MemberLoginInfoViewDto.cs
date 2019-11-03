namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 會員登入可視資料
    /// </summary>
    public class MemberLoginInfoViewDto
    {
        /// <summary>
        /// Gets or sets ServerIP
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// Gets or sets ServerPort
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// Gets or sets Token
        /// </summary>
        public string Token { get; set; }
    }
}