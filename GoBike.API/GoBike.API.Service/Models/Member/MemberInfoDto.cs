namespace GoBike.API.Service.Models.Member
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberInfoDto : MemberBaseDto
    {
        #region 個人資料

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
        /// Gets or sets Gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        public string Photo { get; set; }

        #endregion 個人資料

        #region 互動資料

        /// <summary>
        /// Gets or sets InteractiveStatus (0:等待加入好友請求確認,1:處理加入好友請求，2:好友，-1:黑名單，-2:無互動資料)
        /// </summary>
        public int InteractiveStatus { get; set; }

        #endregion 互動資料
    }
}