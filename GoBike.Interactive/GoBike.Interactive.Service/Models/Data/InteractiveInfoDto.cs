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
        /// Gets or sets InteractiveStatus (-1:黑名單，0:無狀態，1:等待加入好友請求確認，2:處理加入好友請求，3:好友)
        /// </summary>
        public int InteractiveStatus { get; set; }
    }
}