﻿using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Verifier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.API.App.Controllers.Verifier
{
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

        public SendTokenVerifierController(ILogger<SendTokenVerifierController> logger, IVerifierService verifierService)
        {
            this.logger = logger;
            this.verifierService = verifierService;
        }

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