namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 騎乘詳細資訊可視資料
    /// </summary>
    public class RideDetailInfoViewDto : RideSimpleInfoViewDto
    {
        /// <summary>
        /// Gets or sets CityID
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// Gets or sets Climb
        /// </summary>
        public double Climb { get; set; }

        /// <summary>
        /// Gets or sets Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets Level
        /// </summary>
        public int Level { get; set; }
    }
}