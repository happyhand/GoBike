namespace GoBike.Member.Service.Models
{
    /// <summary>
    /// 車輛資料
    /// </summary>
    public class BikeInfoDto
    {
        /// <summary>
        /// Gets or sets BikeID
        /// </summary>
        public string BikeID { get; set; }

        /// <summary>
        /// Gets or sets BuyDate
        /// </summary>
        public string BuyDate { get; set; }

        /// <summary>
        /// Gets or sets ManufactureYaer
        /// </summary>
        public int ManufactureYaer { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets Size
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets Type
        /// </summary>
        public string Type { get; set; }
    }
}