namespace GoBike.API.Service.Models.Verifier.Command
{
    /// <summary>
    /// 驗證碼指令資料
    /// </summary>
    public class VerifierCommandDto
    {
        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets VerifierCode
        /// </summary>
        public string VerifierCode { get; set; }
    }
}