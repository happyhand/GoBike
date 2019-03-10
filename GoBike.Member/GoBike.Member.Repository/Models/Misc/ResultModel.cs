using GoBike.Member.Core.Interface.Models.Misc;

namespace GoBike.Member.Repository.Models.Misc
{
	public class ResultModel : IResultModel
	{
		public int ResultCode { get; set; }
		public string ResultMessage { get; set; }
	}
}