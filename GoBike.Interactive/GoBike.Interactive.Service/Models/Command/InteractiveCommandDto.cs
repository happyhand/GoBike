namespace GoBike.Interactive.Service.Models.Command
{
    /// <summary>
    /// 互動指令資料
    /// </summary>
    public class InteractiveCommandDto
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