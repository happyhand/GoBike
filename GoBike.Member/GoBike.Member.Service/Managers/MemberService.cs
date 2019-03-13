using GoBike.Member.Core.Resource;
using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using GoBike.Member.Repository.Models.Core;
using GoBike.Member.Service.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

		public async Task<Tuple<int, string>> Register(string account, string password)
		{
			try
			{
				bool memberIsExist = await this.memberRepository.GetMemebrData(account) != null;
				if (memberIsExist)
				{
					return Tuple.Create(-1, "The member account is existed.");
				}
				int serialNumber = await this.memberRepository.CreateMemberSerialNumber();
				MemberData member = new MemberData() { ID = serialNumber, Account = account, Password = Utility.EncryptAES(password) };
				Tuple<int, string> result = await this.memberRepository.CreateMember(member);
				switch (result.Item1)
				{
					case 1:
						return Tuple.Create(1, "Register Member Success");

					default:
						return Tuple.Create(0, "Register Member Fail");
				}
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Register Member Error >>>  Account:{account} Password:{password}\n{ex}");
				return Tuple.Create(-999, "Register Member Error");
			}
		}

		public Tuple<int, string> Login(string account, string password)
		{
			return Tuple.Create<int, string>(1, "Login Success");
		}

		public bool CheckLogin(int memberID)
		{
			return true;
		}
	}
}