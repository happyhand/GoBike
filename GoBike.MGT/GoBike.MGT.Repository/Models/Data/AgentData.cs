using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoBike.MGT.Repository.Models.Data
{
    /// <summary>
    /// 代理商資料
    /// </summary>
    public class AgentData
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        [Required]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}