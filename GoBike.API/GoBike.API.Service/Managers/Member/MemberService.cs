using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Managers.Member
{
    /// <summary>
    /// 會員服務
    /// </summary>
    public class MemberService : IMemberService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<MemberService> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public MemberService(ILogger<MemberService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberInfoDto">memberInfoDto</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> EditData(MemberInfoDto memberInfoDto)
        {
            return null;
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> ForgetPassword(string email)
        {
            return null;
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetMemberInfo(string memberID)
        {
            string postData = JsonConvert.SerializeObject(new { MemberID = memberID });
            HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo", postData);
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = httpResponseMessage.Content.ReadAsAsync<MemberInfoDto>().Result
                };
            }

            return new ResponseResultDto()
            {
                Ok = false,
                Data = httpResponseMessage.Content.ReadAsAsync<string>().Result
            };
        }

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Login(string email, string password)
        {
            string postData = JsonConvert.SerializeObject(new { Email = email, Password = password });
            HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Login", postData);
            string result = httpResponseMessage.Content.ReadAsAsync<string>().Result;
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = new LoginInfoDto()
                    {
                        MemberID = result,
                        Token = $"{Utility.EncryptAES(email)}{CommonFlag.SeparateFlag}{Utility.EncryptAES(password)}"
                    }
                };
            }

            return new ResponseResultDto()
            {
                Ok = false,
                Data = result
            };
        }

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Login(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "自動登入驗證碼無效."
                };
            }

            string email = string.Empty;
            string password = string.Empty;
            try
            {
                string[] dataArr = token.Split(CommonFlag.SeparateFlag);
                email = Utility.DecryptAES(dataArr[0]);
                password = Utility.DecryptAES(dataArr[1]);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Token Login Error >>> Token:{token}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "自動登入驗證碼發生錯誤，無法編譯."
                };
            }

            return await this.Login(email, password);
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Register(string email, string password)
        {
            string postData = JsonConvert.SerializeObject(new { Email = email, Password = password });
            HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Register", postData);
            return new ResponseResultDto()
            {
                Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                Data = httpResponseMessage.Content.ReadAsAsync<string>().Result
            };
        }
    }
}