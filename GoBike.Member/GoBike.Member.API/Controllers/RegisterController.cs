using GoBike.Member.API.Models.Response;
using GoBike.Member.Core.Interface.Models.Misc;
using GoBike.Member.Core.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoBike.Member.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : Controller
	{
		private readonly ILogger<RegisterController> logger;
		private readonly IMemberService memberService;

		public RegisterController(ILogger<RegisterController> logger, IMemberService memberService)
		{
			this.logger = logger;
			this.memberService = memberService;
		}

		[HttpGet]
		public string Get(string account, string password)
		{
			return "ok";
		}

		[HttpPost]
		public ResultModel Post(DataObj data)
		{
			this.logger.LogInformation($"Account:{data.account}  Password:{data.password}");
			IResultModel result = this.memberService.Register(data.account, data.password);
			switch (result.ResultCode)
			{
				case 1:
					return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage };

				default:
					return new ResultModel() { ResultCode = result.ResultCode, ResultMessage = result.ResultMessage };
			}
		}

		public class DataObj
		{
			public string account { get; set; }
			public string password { get; set; }
		}
	}
}