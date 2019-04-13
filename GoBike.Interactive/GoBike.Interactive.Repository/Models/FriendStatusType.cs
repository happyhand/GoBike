namespace GoBike.Interactive.Repository.Models
{
    public enum FriendStatusType
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
        /// 請求、等待確認
        /// </summary>
        Request = 0,

        /// <summary>
        /// 好友
        /// </summary>
        Friend = 1
    }
}