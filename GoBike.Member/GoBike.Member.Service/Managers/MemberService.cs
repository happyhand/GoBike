using GoBike.Member.Core.Interface.Models.Misc;
using GoBike.Member.Core.Interface.Repository;
using GoBike.Member.Core.Interface.Service;
using GoBike.Member.Service.Models.Misc;
using Microsoft.Extensions.Logging;

namespace GoBike.Member.Service.Managers
{
	public class MemberService : IMemberService
	{
		private readonly ILogger<MemberService> logger;
		private readonly IMemberRepository memberRepository;

		public MemberService(ILogger<MemberService> logger, IMemberRepository memberRepository)
		{
			this.logger = logger;
			this.memberRepository = memberRepository;
		}

		public IResultModel Register(string account, string password)
		{
			this.logger.LogInformation($"Account:{account}  Password:{password}");
			IResultModel result = this.memberRepository.RegisterMember(account, password);
			return result;
		}

		public IResultModel Login(string account, string password)
		{
			return new ResultModel() { ResultCode = 1, ResultMessage = "Login Success" };
		}

		public bool CheckLogin(int memberID)
		{
			return true;
		}
	}
}