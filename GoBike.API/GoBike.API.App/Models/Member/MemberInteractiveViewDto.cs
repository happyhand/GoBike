namespace GoBike.API.App.Models.Member
{
    /// <summary>
    /// 會員可視資料
    /// </summary>
    public class MemberInteractiveViewDto : MemberViewDto
    {
        /// <summary>
        /// Gets or sets Status (0:等待確認，1:好友，-1:黑名單，-2:無互動資料)
        /// </summary>
        public int Status { get; set; }
    }
}