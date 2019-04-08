namespace GoBike.API.Service.Email
{
    /// <summary>
    /// 郵件資料
    /// </summary>
    public class EmailContext
    {
        /// <summary>
        /// Gets or sets Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets Subject
        /// </summary>
        public string Subject { get; set; }
    }
}