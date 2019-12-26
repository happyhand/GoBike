namespace GoBike.Service.Core.Resource.Enum
{
    /// <summary>
    /// 性別類別資料
    /// </summary>
    public enum GenderType
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 女生
        /// </summary>
        Woman = 1,

        /// <summary>
        /// 男生
        /// </summary>
        Man = 2
    }

    /// <summary>
    /// 會員互動類別資料
    /// </summary>
    public enum InteractiveType
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 好友
        /// </summary>
        Friend = 1,

        /// <summary>
        /// 黑名單
        /// </summary>
        Black = 2
    }

    /// <summary>
    /// 行動電話綁定類別資料
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