using System;

namespace GoBike.Service.Repository.Models.Member
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberDto
    {
        #region Register \ Login Data

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

        #endregion Register \ Login Data

        #region Info Data

        /// <summary>
        /// Gets or sets Birthday
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        public double BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        public double BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets FrontCoverUrl
        /// </summary>
        public string FrontCoverUrl { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets MoblieBindType
        /// </summary>
        public int MoblieBindType { get; set; }

        /// <summary>
        /// Gets or sets NoticeType
        /// </summary>
        public int NoticeType { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        public string PhotoUrl { get; set; }

        #endregion Info Data

        #region Extra Data

        /// <summary>
        /// Gets or sets LatestRideDistance
        /// </summary>
        public double LatestRideDistance { get; set; }

        /// <summary>
        /// Gets or sets TotalRideDistance
        /// </summary>
        public double TotalRideDistance { get; set; }

        #endregion Extra Data
    }
}