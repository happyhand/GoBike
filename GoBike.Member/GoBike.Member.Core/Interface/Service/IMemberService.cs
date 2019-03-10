using GoBike.Member.Core.Interface.Models.Misc;

namespace GoBike.Member.Core.Interface.Service
{
	public interface IMemberService
	{
		IResultModel Register(string account, string password);

		IResultModel Login(string account, string password);

		bool CheckLogin(int memberID);
	}
}