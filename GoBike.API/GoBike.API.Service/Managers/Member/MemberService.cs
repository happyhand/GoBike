using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Email;
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
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> EditData(MemberInfoDto memberInfo)
        {
            if (string.IsNullOrEmpty(memberInfo.MemberID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(memberInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/EditData", postData);
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
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> EditData:{Utility.GetPropertiesData(memberInfo)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員更新資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetMemberInfo(string memberID)
        {
            if (string.IsNullOrEmpty(memberID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
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
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info Error >>> MemberID:{memberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得會員資訊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱或密碼無效."
                };
            }

            try
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
                            Token = $"{Utility.EncryptAES(email)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(password)}"
                        }
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Error >>> Email:{email} Password:{password}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員登入發生錯誤."
                };
            }
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
                string[] dataArr = token.Split(CommonFlagHelper.CommonFlag.SeparateFlag);
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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱或密碼無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(new { Email = email, Password = password });
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Register", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = httpResponseMessage.Content.ReadAsAsync<string>().Result
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Email:{email} Password:{password}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員註冊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<ResponseResultDto> ResetPassword(string email)
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
                string postData = JsonConvert.SerializeObject(new { Email = email });
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/ResetPassword", postData);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    string password = httpResponseMessage.Content.ReadAsAsync<string>().Result;
                    EmailContext emailContext = this.GetResetPasswordEmailContext(email, password);
                    postData = JsonConvert.SerializeObject(emailContext);
                    httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.SmtpService, "api/SendEmail", postData);

                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        return new ResponseResultDto()
                        {
                            Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                            Data = httpResponseMessage.Content.ReadAsAsync<string>().Result
                        };
                    }
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = httpResponseMessage.Content.ReadAsAsync<string>().Result
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reset Password Error >>> Email:{email}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "重設密碼驗證發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得重設密碼郵件內容
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>EmailContext</returns>
        private EmailContext GetResetPasswordEmailContext(string email, string password)
        {
            return new EmailContext()
            {
                Address = email,
                Body = $"<p>親愛的用戶您好</p>" +
                       $"<p>您於 <span style='font-weight:bold; color:blue;'>{DateTime.Now:yyyy/MM/dd HH:mm:ss}</span> 重設密碼</p>" +
                       $"<p>您的密碼更新為</p>" +
                       $"<p><span style='font-weight:bold; color:blue;'>{password}</span></p>" +
                       $"<p>請於重新登入後，於設定個人檔案重新設定密碼</p>" +
                       $"<br><br><br>" +
                       $"<p>※本電子郵件係由系統自動發送，請勿直接回覆本郵件。</p>",
                Subject = "GoBike 查詢密碼"
            };
        }
    }
}