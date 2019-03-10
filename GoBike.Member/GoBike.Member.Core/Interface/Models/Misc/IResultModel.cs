using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Member.Core.Interface.Models.Misc
{
	public interface IResultModel
	{
		int ResultCode { get; set; }
		string ResultMessage { get; set; }
	}
}