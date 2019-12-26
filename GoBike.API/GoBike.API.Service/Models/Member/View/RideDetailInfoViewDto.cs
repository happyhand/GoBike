using System.Collections.Generic;

namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 騎乘詳細資訊可視資料
    /// </summary>
    public class RideDetailInfoViewDto : RideSimpleInfoViewDto
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
        /// Gets or sets Level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets Route
        /// </summary>
        public IEnumerable<RideRouteInfoViewDto> Route { get; set; }

        /// <summary>
        /// Gets or sets ShareContent
        /// </summary>
        public IEnumerable<RideContentInfoViewDto> ShareContent { get; set; }

        /// <summary>
        /// Gets or sets SharedType
        /// </summary>
        public int SharedType { get; set; }

        /// <summary>
        /// 騎乘路徑資料
        /// </summary>
        public class RouteDto
        {
            /// <summary>
            /// Gets or sets Latitude
            /// </summary>
            public string Latitude { get; set; }

            /// <summary>
            /// Gets or sets Longitude
            /// </summary>
            public string Longitude { get; set; }
        }
    }
}