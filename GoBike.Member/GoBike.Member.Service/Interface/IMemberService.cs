using System;
using System.Threading.Tasks;

namespace GoBike.Member.Service.Interface
{
	public interface IMemberService
	{
		Task<Tuple<int, string>> Register(string account, string password);

		Tuple<int, string> Login(string account, string password);

		bool CheckLogin(int memberID);
	}
}