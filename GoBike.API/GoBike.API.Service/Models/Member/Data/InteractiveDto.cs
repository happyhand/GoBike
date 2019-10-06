namespace GoBike.API.Service.Models.Member.Data
{
    /// <summary>
    /// 互動資料
    /// </summary>
    public class InteractiveDto
    {
        /// <summary>
        /// Gets or sets InteractiveID
        /// </summary>
        public string InteractiveID { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public int Status { get; set; }

        #region Extra Data

        /// <summary>
        /// Gets or sets SearchKey
        /// </summary>
        public string SearchKey { get; set; }

        #endregion Extra Data
    }
}