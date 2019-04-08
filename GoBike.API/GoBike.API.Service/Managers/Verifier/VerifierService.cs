using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Repository.Interface;
using GoBike.API.Service.Email;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
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
        /// 驗證碼是否有效
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="verifierCode">verifierCode</param>
        /// <returns>bool</returns>
        public async Task<bool> IsValidVerifierCode(string email, string verifierCode)
        {
            string cacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.VerifierCode}-{email}-{verifierCode}";
            string data = await this.redisRepository.GetCache(cacheKey);
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 發送驗證碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SendVerifierCode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱無效."
                };
            }

            string verifierCode = await this.GetVerifierCode(email);
            EmailContext emailContext = this.GetVerifierCodetEmailContext(email, verifierCode);
            string postData = JsonConvert.SerializeObject(emailContext);
            HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.SmtpService, "api/SendEmail", postData);
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                string cacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.VerifierCode}-{email}-{verifierCode}";
                bool isSetCache = await this.redisRepository.SetCache(cacheKey, verifierCode, new TimeSpan(0, 10, 0));
                if (isSetCache)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = verifierCode
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
                Data = httpResponseMessage.Content.ReadAsAsync<string>().Result
            };
        }

        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>string</returns>
        private async Task<string> GetVerifierCode(string email)
        {
            string fuzzyCacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.VerifierCode}-{email}-*";
            string cacheKey = this.redisRepository.GetRedisKeys(fuzzyCacheKey).FirstOrDefault();
            if (string.IsNullOrEmpty(cacheKey))
            {
                return Guid.NewGuid().ToString().Substring(0, 6);
            }

            return await this.redisRepository.GetCache(cacheKey);
        }

        /// <summary>
        /// 取得驗證碼郵件內容
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="verifierCode">verifierCode</param>
        /// <returns>EmailContext</returns>
        private EmailContext GetVerifierCodetEmailContext(string email, string verifierCode)
        {
            return new EmailContext()
            {
                Address = email,
                Body = $"<p>親愛的用戶您好</p>" +
                       $"<p>您於 <span style='font-weight:bold; color:blue;'>{DateTime.Now:yyyy/MM/dd HH:mm:ss}</span> 查詢密碼</p>" +
                       $"<p>您的查詢驗證碼為</p>" +
                       $"<p><span style='font-weight:bold; color:blue;'>{verifierCode}</span></p>" +
                       $"<p>請於 <span style='font-weight:bold; color:blue;'>10分鐘</span> 內於APP輸入此驗證碼以獲取新密碼</p>" +
                       $"<br><br><br>" +
                       $"<p>※本電子郵件係由系統自動發送，請勿直接回覆本郵件。</p>",
                Subject = "GoBike 查詢密碼"
            };
        }
    }
}