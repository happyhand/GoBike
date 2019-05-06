using System;

namespace GoBike.Member.Service.Models
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberInfoDto
    {
        #region Base Data

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email { get; set; }

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

        #endregion Base Data

        #region Info Data

        /// <summary>
        /// Gets or sets BirthDayDate
        /// </summary>
        public string BirthDayDate { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        public decimal BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        public decimal BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets Gender (0:Girl, 1:Boy)
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        public string Photo { get; set; }

        #endregion Info Data
    }
}