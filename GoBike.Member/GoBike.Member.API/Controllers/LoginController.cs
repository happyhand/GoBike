using GoBike.Member.API.Models.Response;
using GoBike.Member.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
		public ResultModel Post(RequestData requestData)
		{
			Tuple<int, string> result = this.memberService.Login(requestData.Account, requestData.Password);
			switch (result.Item1)
			{
				case 1:
					return new ResultModel() { ResultCode = result.Item1, ResultMessage = result.Item2 };

				default:
					return new ResultModel() { ResultCode = result.Item1, ResultMessage = result.Item2 };
			}
		}

		public class RequestData
		{
			public string Account { get; set; }
			public string Password { get; set; }
		}
	}
}