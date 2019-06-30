namespace GoBike.API.Service.Models.Interactive.Command
{
    /// <summary>
    /// 會員互動指令資料
    /// </summary>
    public class MemberInteractiveCommandDto
    {
        /// <summary>
        /// Gets or sets InitiatorID
        /// </summary>
        public string InitiatorID { get; set; }

        /// <summary>
        /// Gets or sets ReceiverID
        /// </summary>
        public string ReceiverID { get; set; }
    }
}