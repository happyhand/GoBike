using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Repository.Interface;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Managers.Verifier
{
    /// <summary>
    /// 驗證碼服務
    /// </summary>
    public class VerifierService : IVerifierService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<VerifierService> logger;

        /// <summary>
        /// redisRepository
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="redisRepository">redisRepository</param>
        public VerifierService(ILogger<VerifierService> logger, IRedisRepository redisRepository)
        {
            this.logger = logger;
            this.redisRepository = redisRepository;
        }

        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="email">email</param>
        /// <returns>string</returns>
        public async Task<string> GetVerifierCode(string type, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return string.Empty;
                }

                string fuzzyCacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.VerifierCode}-{type}-{email}-*";
                string cacheKey = this.redisRepository.GetRedisKeys(fuzzyCacheKey).FirstOrDefault();
                if (!string.IsNullOrEmpty(cacheKey))
                {
                    return await this.redisRepository.GetCache(cacheKey);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Verifier Code Error >>> Type:{type} Email:{email}\n{ex}");
                return string.Empty;
            }

            return Guid.NewGuid().ToString().Substring(0, 6);
        }

        /// <summary>
        /// 驗證碼是否有效
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="email">email</param>
        /// <param name="VerifierCode">VerifierCode</param>
        /// <returns>bool</returns>
        public async Task<bool> IsValidVerifierCode(string type, string email, string VerifierCode)
        {
            try
            {
                string cacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.VerifierCode}-{type}-{email}-{VerifierCode}";
                string data = await this.redisRepository.GetCache(cacheKey);
                if (string.IsNullOrEmpty(data))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Is Valid Verifier Code Error >>> Type:{type} Email:{email} VerifierCode:{VerifierCode}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 發送驗證碼
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="email">email</param>
        /// <param name="VerifierCode">VerifierCode</param>
        /// <param name="emailContext">emailContext</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SendVerifierCode(string type, string email, string VerifierCode, EmailContext emailContext)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(emailContext);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.SmtpService, "api/SendEmail", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string cacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.VerifierCode}-{type}-{email}-{VerifierCode}";
                    bool isSetCache = await this.redisRepository.SetCache(cacheKey, VerifierCode, TimeSpan.FromMinutes(10));
                    if (isSetCache)
                    {
                        return new ResponseResultDto()
                        {
                            Ok = true,
                            Data = "發送驗證碼成功."
                        };
                    }

                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = "存取驗證碼失敗."
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Send Verifier Code Error >>> Type:{type} Email:{email} VerifierCode:{VerifierCode} EmailContext:{JsonConvert.SerializeObject(emailContext)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "發送驗證碼發生錯誤."
                };
            }
        }
    }
}