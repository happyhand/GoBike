namespace GoBike.API.Core.Resource.Enum
{
    /// <summary>
    /// 手機綁定類別資料
    /// </summary>
    public enum MoblieBindType
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 綁定
        /// </summary>
        Bind = 1
    }

    /// <summary>
    /// 在線狀態類別資料
    /// </summary>
    public enum OnlineStatusType
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None = -1,

        /// <summary>
        /// 未上線
        /// </summary>
        Offline = 0,

        /// <summary>
        /// 在線
        /// </summary>
        Online = 1
    }

    /// <summary>
    /// 騎乘資料分享類別資料
    /// </summary>
    public enum RideSharedType
    {
        /// <summary>
        /// 不分享
        /// </summary>
        None = 0,

        /// <summary>
        /// 分享
        /// </summary>
        Shared = 1
    }
}