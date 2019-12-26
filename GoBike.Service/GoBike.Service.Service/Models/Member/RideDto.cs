using System;
using System.Collections.Generic;

namespace GoBike.Service.Service.Models.Member
{
    /// <summary>
    /// 騎乘資料
    /// </summary>
    public class RideDto
    {
        /// <summary>
        /// Gets or sets Altitude
        /// </summary>
        public string Altitude { get; set; }

        /// <summary>
        /// Gets or sets CountyID
        /// </summary>
        public int CountyID { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Distance
        /// </summary>
        public string Distance { get; set; }

        /// <summary>
        /// Gets or sets Level
        /// </summary>
        public int Level { get; set; }

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
        /// Gets or sets Route
        /// </summary>
        public IEnumerable<RideRouteDto> Route { get; set; }

        /// <summary>
        /// Gets or sets ShareContent
        /// </summary>
        public IEnumerable<RideContentDto> ShareContent { get; set; }

        /// <summary>
        /// Gets or sets SharedType
        /// </summary>
        public int SharedType { get; set; }

        /// <summary>
        /// Gets or sets Time
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public string Title { get; set; }
    }
}