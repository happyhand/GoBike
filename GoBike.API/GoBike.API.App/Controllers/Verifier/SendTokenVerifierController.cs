using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Verifier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Verifier
{
    /// <summary>
    /// 發送驗證碼
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SendTokenVerifierController : ApiController
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SendTokenVerifierController> logger;

        /// <summary>
        /// verifierService
        /// </summary>
        private readonly IVerifierService verifierService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="verifierService">verifierService</param>
        public SendTokenVerifierController(ILogger<SendTokenVerifierController> logger, IVerifierService verifierService)
        {
            this.logger = logger;
            this.verifierService = verifierService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="verifierInfoDto">verifierInfoDto</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(VerifierInfoDto verifierInfoDto)
        {
            try
            {
                ResponseResultDto responseResultDto = await this.verifierService.SendVerifierCode(verifierInfoDto, null);
                if (responseResultDto.Ok)
                {
                    return Ok(responseResultDto.Data);
                }

                return BadRequest(responseResultDto.Data);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Send Token Verifier Error >>> Email:{verifierInfoDto.Email}\n{ex}");
                return BadRequest("取得會員資訊發生錯誤.");
            }
        }
    }
}