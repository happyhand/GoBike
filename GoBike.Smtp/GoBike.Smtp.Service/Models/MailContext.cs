namespace GoBike.Smtp.Service.Models
{
    public class MailContext
    {
        /// <summary>
        /// Gets or sets Addressee
        /// </summary>
        public string Addressee { get; set; }

        /// <summary>
        /// Gets or sets Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets EmailAddress
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets Subject
        /// </summary>
        public string Subject { get; set; }
    }
}