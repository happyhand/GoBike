using GoBike.Member.API.Models.Response;
using GoBike.Member.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

		[HttpPost]
		public async Task<ResultModel> Post(RequestData requestData)
		{
			this.logger.LogInformation($"Register Member >>> Account:{requestData.Account}  Password:{requestData.Password}");
			Tuple<int, string> result = await this.memberService.Register(requestData.Account, requestData.Password);
			return new ResultModel() { ResultCode = result.Item1, ResultMessage = result.Item2, ResultData = result.Item2 };
		}

		public class RequestData
		{
			public string Account { get; set; }
			public string Password { get; set; }
		}
	}
}