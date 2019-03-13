using GoBike.Member.API.Models.Response;
using GoBike.Member.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GoBike.Member.API.Filters.Action
{
	public class CheckLoginActionFilter : ActionFilterAttribute, IActionFilter
	{
		private readonly ILogger<CheckLoginActionFilter> logger;
		private readonly IMemberService memberService;

		public CheckLoginActionFilter(ILogger<CheckLoginActionFilter> logger, IMemberService memberService)
		{
			this.logger = logger;
			this.memberService = memberService;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!this.memberService.CheckLogin(0))
			{
				ResultModel resultModel = new ResultModel()
				{
					ResultCode = -999,
					ResultMessage = "Login Fail"
				};
				context.Result = new JsonResult(resultModel);
				this.logger.LogError($"Check Result >>> {resultModel.ResultMessage}");
			}
		}
	}
}