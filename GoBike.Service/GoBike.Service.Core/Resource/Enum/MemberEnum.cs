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
    /// 通知設定類別資料
    /// </summary>
    public enum NoticeSettingType
    {
        /// <summary>
        /// 組隊邀請通知
        /// </summary>
        RideInviteNotice = 1,

        /// <summary>
        /// 車隊邀請通知
        /// </summary>
        TeamInviteNotice = 2,

        /// <summary>
        /// 好友邀請通知
        /// </summary>
        FriendInviteNotice = 4,

        /// <summary>
        /// 優惠通知
        /// </summary>
        OfferNotice = 8,

        /// <summary>
        /// 活動通知
        /// </summary>
        ActivityNotice = 16
    }
}