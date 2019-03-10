using GoBike.Member.API.Models.Response;
using GoBike.Member.Core.Interface.Models.Misc;
using GoBike.Member.Core.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoBike.Member.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : Controller
	{
		private readonly ILogger logger;
		private readonly IMemberService memberService;

		public LoginController(ILogger<LoginController> logger, IMemberService memberService)
		{
			this.logger = logger;
			this.memberService = memberService;
		}

		[HttpPost]
		public ResultModel Post(string account, string password)
		{
			IResultModel result = this.memberService.Login(account, password);
			switch (result.ResultCode)
			{
				case 1:
					return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage };

				default:
					return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage };
			}
		}
	}
}