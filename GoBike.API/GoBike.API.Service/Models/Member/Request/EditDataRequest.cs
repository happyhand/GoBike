namespace GoBike.API.Service.Models.Response
{
    /// <summary>
    /// 會員修改回應資料
    /// </summary>
    public class EditDataRequest
    {
        /// <summary>
        /// Gets or sets BirthDayDate
        /// </summary>
        public string BirthDayDate { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        public string BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        public string BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }
    }
}