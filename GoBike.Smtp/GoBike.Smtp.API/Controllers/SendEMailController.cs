using GoBike.Smtp.Core.Resource;
using GoBike.Smtp.Service.Interface;
using GoBike.Smtp.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.Smtp.API.Controllers
{
    /// <summary>
    /// 發送郵件 API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SendEmailController> logger;

        /// <summary>
        /// smtpService
        /// </summary>
        private readonly ISmtpService smtpService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="smtpService">smtpService</param>
        public SendEmailController(ILogger<SendEmailController> logger, ISmtpService smtpService)
        {
            this.logger = logger;
            this.smtpService = smtpService;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="emailContext">emailContext</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(EmailContext emailContext)
        {
            try
            {
                string result = await this.smtpService.SendEmail(emailContext);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("發送郵件成功.");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Send Email Error Error >>> Data:{Utility.GetPropertiesData(emailContext)}\n{ex}");
                return BadRequest("發送郵件發生錯誤.");
            }
        }
    }
}