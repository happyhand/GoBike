using System;

namespace GoBike.Service.Repository.Models.Member
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberDto
    {
        #region Login Data

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets FBToken
        /// </summary>
        public string FBToken { get; set; }

        /// <summary>
        /// Gets or sets GoogleToken
        /// </summary>
        public string GoogleToken { get; set; }

        /// <summary>
        /// Gets or sets LoginDate
        /// </summary>
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        public string Password { get; set; }

        #endregion Login Data

        #region Info Data

        /// <summary>
        /// Gets or sets LatestRideDistance
        /// </summary>
        public double LatestRideDistance { get; set; }

        /// <summary>
        /// Gets or sets TotalRideDistance
        /// </summary>
        public double TotalRideDistance { get; set; }

        #endregion Info Data
    }
}