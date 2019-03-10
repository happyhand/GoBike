using System.Collections.Generic;
using GoBike.Member.Core.Interface.Models.Member;
using GoBike.Member.Core.Interface.Models.Misc;
using GoBike.Member.Core.Interface.Repository;
using GoBike.Member.Repository.Models;
using GoBike.Member.Repository.Models.Misc;

namespace GoBike.Member.Repository.Managers
{
	public class MemberRepository : IMemberRepository
	{
		public IResultModel RegisterMember(string account, string password)
		{
			return new ResultModel() { ResultCode = 1, ResultMessage = "Register Success" };
		}

		public IMemberModel GetMemebrData(int memberID)
		{
			return new MemberModel() { ID = 10001, Account = "admin", Password = "123456" };
		}

		public IEnumerable<IMemberModel> GetMemebrDatas(IEnumerable<int> memberIDs)
		{
			return new List<MemberModel>()
			{
				 new MemberModel() { ID = 10001, Account = "admin", Password = "123456" }
			};
		}
	}
}