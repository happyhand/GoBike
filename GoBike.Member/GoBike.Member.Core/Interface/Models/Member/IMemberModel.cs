using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Member.Core.Interface.Models.Member
{
	public interface IMemberModel
	{
		int ID { get; set; }
		string Account { get; set; }
		string Password { get; set; }
	}
}