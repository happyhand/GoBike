using GoBike.Service.Repository.Models.Member;
using GoBike.Service.Service.Interface.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.API.Controllers.Member
{
    /// <summary>
    /// 會員編輯
    /// </summary>
    [Route("api/Member/[controller]")]
    [ApiController]
    public class EditDataController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EditDataController> logger;

        /// <summary>
        /// memberService
        /// </summary>
        private readonly IMemberService memberService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberService">memberService</param>
        public EditDataController(ILogger<EditDataController> logger, IMemberService memberService)
        {
            this.logger = logger;
            this.memberService = memberService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(MemberDto memberDto)
        {
            try
            {
                string result = await this.memberService.EditData(memberDto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("會員編輯成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(memberDto)}\n{ex}");
                return BadRequest("會員編輯發生錯誤");
            }
        }
    }
}