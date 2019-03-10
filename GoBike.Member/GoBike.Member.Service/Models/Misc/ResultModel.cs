using GoBike.Member.Core.Interface.Models.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Member.Service.Models.Misc
{
	public class ResultModel : IResultModel
	{
		public int ResultCode { get; set; }
		public string ResultMessage { get; set; }
	}
}