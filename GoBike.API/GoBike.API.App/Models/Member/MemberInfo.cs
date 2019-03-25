namespace GoBikeAPI.App.Models.Member
{
    public class MemberInfo
    {
        /// <summary>
        /// Gets or sets BirthDayDate
        /// </summary>
        public string BirthDayDate { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        public decimal BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        public decimal BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Gender (0:Girl, 1:Boy)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }
    }
}