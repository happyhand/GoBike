namespace GoBike.API.Service.Models.Response
{
    /// <summary>
    /// 會員登入回應資料
    /// </summary>
    public class LoginRespone
    {
        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets ResultCode
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets ResultMessage
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets Token
        /// </summary>
        public string Token { get; set; }
    }
}