using GoBike.Member.Core.Interface.Models.Member;
using System;

namespace GoBike.Member.Repository.Models
{
	public class MemberModel : IMemberModel
	{
		public int ID { get; set; }
		public string Account { get; set; }
		public string Password { get; set; }
	}
}