using GoBike.Member.Repository.Models;
using GoBike.Member.Repository.Models.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.Repository.Interface
{
	public interface IMemberRepository
	{
		Task<Tuple<int, string>> CreateMember(MemberData memberData);

		Task<MemberData> GetMemebrData(int id);

		Task<MemberData> GetMemebrData(string account);

		Task<int> CreateMemberSerialNumber();
	}
}