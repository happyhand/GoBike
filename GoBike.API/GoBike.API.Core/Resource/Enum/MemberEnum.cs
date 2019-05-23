namespace GoBike.API.Core.Resource.Enum
{
    /// <summary>
    /// 互動狀態資料
    /// </summary>
    public enum InteractiveStatusType
    {
        /// <summary>
        /// 黑名單
        /// </summary>
        Black = -1,

        /// <summary>
        /// 無狀態
        /// </summary>
        None = 0,

        /// <summary>
        /// 請求 - 等待確認
        /// </summary>
        Request = 1,

        /// <summary>
        /// 請求 - 處理確認
        /// </summary>
        RequestHandler = 2,

        /// <summary>
        /// 好友
        /// </summary>
        Friend = 3
    }
}