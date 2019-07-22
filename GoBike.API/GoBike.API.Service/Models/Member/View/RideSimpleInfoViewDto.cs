using System;

namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 騎乘簡易資訊可視資料
    /// </summary>
    public class RideSimpleInfoViewDto
    {
        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Distance
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Gets or sets MapUrl
        /// </summary>
        public string MapUrl { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets RideID
        /// </summary>
        public string RideID { get; set; }

        /// <summary>
        /// Gets or sets RideTime
        /// </summary>
        public long RideTime { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public string Title { get; set; }
    }
}