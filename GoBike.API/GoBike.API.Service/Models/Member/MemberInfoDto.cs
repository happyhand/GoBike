﻿namespace GoBike.API.Service.Models.Member
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberInfoDto
    {
        /// <summary>
        /// Gets or sets BirthDayDate
        /// </summary>
        public string BirthDayDate { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        public decimal? BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        public decimal? BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        public int? Gender { get; set; }

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

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        public string Password { get; set; }
    }
}