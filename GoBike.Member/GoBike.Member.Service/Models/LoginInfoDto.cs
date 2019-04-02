namespace GoBike.Member.Service.Models
{
    public class LoginInfoDto
    {
        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public int Status { get; set; }
    }
}