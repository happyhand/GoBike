namespace GoBike.API.Service.Models.Verifier.Data
{
    /// <summary>
    /// 驗證碼資料
    /// </summary>
    public class VerifierDto
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