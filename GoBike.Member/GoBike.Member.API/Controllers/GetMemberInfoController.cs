using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.API.Controllers
{
	/// <summary>
	/// 取得會員資訊
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class GetMemberInfoController : ControllerBase
	{
		/// <summary>
		/// logger
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// memberService
		/// </summary>
		private readonly IMemberService memberService;

		/// <summary>
		/// 建構式
		/// </summary>
		/// <param name="logger">logger</param>
		/// <param name="memberService">memberService</param>
		public GetMemberInfoController(ILogger<GetMemberInfoController> logger, IMemberService memberService)
		{
			this.logger = logger;
			this.memberService = memberService;
		}

		/// <summary>
		/// POST
		/// </summary>
		/// <param name="inputData">inputData</param>
		/// <returns>IActionResult</returns>
		[HttpPost]
		public async Task<IActionResult> Post(InputData inputData)
		{
			try
			{
				Tuple<MemberInfoDto, string> result = await this.memberService.GetMemberInfo(inputData.MemberID);
				if (string.IsNullOrEmpty(result.Item2))
				{
					return Ok(result.Item1);
				}

				return BadRequest(result.Item2);
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Get Member Info Error >>> MemberID:{inputData.MemberID}\n{ex}");
				return BadRequest("取得會員資訊發生錯誤");
			}
		}

		/// <summary>
		/// 請求資料
		/// </summary>
		public class InputData
		{
			/// <summary>
			/// Gets or sets MemberID
			/// </summary>
			public string MemberID { get; set; }
		}
	}
}