namespace GoBike.API.Service.Models.Data
{
    /// <summary>
    /// 互動狀態資料
    /// </summary>
    public enum InteractiveStatusType
    {
        /// <summary>
        /// 無關聯
        /// </summary>
        None = -2,

        /// <summary>
        /// 黑名單
        /// </summary>
        Black = -1,

        /// <summary>
        /// 請求 - 等待確認
        /// </summary>
        Request = 0,

        /// <summary>
        /// 請求 - 處理確認
        /// </summary>
        RequestHandler = 1,

        /// <summary>
        /// 好友
        /// </summary>
        Friend = 2
    }

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