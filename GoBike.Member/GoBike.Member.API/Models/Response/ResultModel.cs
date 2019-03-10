using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Member.API.Models.Response
{
	public class ResultModel
	{
		public int ResultCode { get; set; }
		public string ResultMessage { get; set; }
		public dynamic ResultData { get; set; }
	}
}