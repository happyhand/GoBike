namespace GoBike.Interactive.Service.Models.Data
{
    /// <summary>
    /// 互動資料
    /// </summary>
    public class InteractiveInfoDto
    {
        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Status (0:等待加入好友請求確認,1:處理加入好友請求，2:好友，-1:黑名單，-2:無互動資料)
        /// </summary>
        public int Status { get; set; }
    }
}