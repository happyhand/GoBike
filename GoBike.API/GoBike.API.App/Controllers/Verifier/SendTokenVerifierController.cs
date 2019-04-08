using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBikeAPI.App.Controllers.Verifier
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendTokenVerifierController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SendTokenVerifierController> logger;

        /// <summary>
        /// verifierService
        /// </summary>
        private readonly IVerifierService verifierService;

        public SendTokenVerifierController(ILogger<SendTokenVerifierController> logger, IVerifierService verifierService)
        {
            this.logger = logger;
            this.verifierService = verifierService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(InputData inputData)
        {
            try
            {
                ResponseResultDto responseResultDto = await this.verifierService.SendVerifierCode(inputData.Email);
                if (responseResultDto.Ok)
                {
                    return Ok(responseResultDto.Data);
                }

                return BadRequest(responseResultDto.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Send Token Verifier Error >>> Email:{inputData.Email}\n{ex}");
                return BadRequest("取得會員資訊發生錯誤.");
            }
        }

        /// <summary>
        /// 請求資料
        /// </summary>
        public class InputData
        {
            /// <summary>
            /// Gets or sets Email
            /// </summary>
            public string Email { get; set; }
        }
    }
}