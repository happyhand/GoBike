using GoBike.Member.Core.Interface.Models.Member;
using GoBike.Member.Core.Interface.Models.Misc;
using System.Collections.Generic;

namespace GoBike.Member.Core.Interface.Repository
{
	public interface IMemberRepository
	{
		IResultModel RegisterMember(string account, string password);

		IMemberModel GetMemebrData(int memberID);

		IEnumerable<IMemberModel> GetMemebrDatas(IEnumerable<int> memberIDs);
	}
}