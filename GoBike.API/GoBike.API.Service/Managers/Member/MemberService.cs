using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Email;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<MemberInfoDto>()
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
                this.logger.LogError($"Edit Data Error >>> EditData:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員更新資訊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetMemberInfo(MemberInfoDto memberInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(memberInfo.MemberID) && string.IsNullOrEmpty(memberInfo.Email) && string.IsNullOrEmpty(memberInfo.Mobile))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = "無效的查詢參數."
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<MemberInfoDto>()
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
                this.logger.LogError($"Get Member Info Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
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
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Login(MemberInfoDto memberInfo)
        {
            if (string.IsNullOrEmpty(memberInfo.Email) || string.IsNullOrEmpty(memberInfo.Password))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱或密碼無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(memberInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Login", postData);
                string result = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = new LoginInfoDto()
                        {
                            MemberID = result,
                            Token = $"{Utility.EncryptAES(memberInfo.Email)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(memberInfo.Password)}"
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
                this.logger.LogError($"Login Error >>> Email:{memberInfo.Email} Password:{memberInfo.Password}\n{ex}");
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

            return await this.Login(new MemberInfoDto() { Email = email, Password = password });
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Register(MemberInfoDto memberInfo)
        {
            if (string.IsNullOrEmpty(memberInfo.Email) || string.IsNullOrEmpty(memberInfo.Password))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱或密碼無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(memberInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Register", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Email:{memberInfo.Email} Password:{memberInfo.Password}\n{ex}");
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
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<ResponseResultDto> ResetPassword(MemberInfoDto memberInfo)
        {
            if (string.IsNullOrEmpty(memberInfo.Email))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱無效."
                };
            }

            try
            {
                memberInfo.Password = Guid.NewGuid().ToString().Substring(0, 8);
                string postData = JsonConvert.SerializeObject(memberInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/ResetPassword", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    EmailContext emailContext = EmailContext.GetResetPasswordEmailContext(memberInfo.Email, memberInfo.Password);
                    postData = JsonConvert.SerializeObject(emailContext);
                    httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.SmtpService, "api/SendEmail", postData);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        return new ResponseResultDto()
                        {
                            Ok = httpResponseMessage.IsSuccessStatusCode,
                            Data = "已重設密碼，並發送郵件成功."
                        };
                    }
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reset Password Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "重設密碼驗證發生錯誤."
                };
            }
        }

        /// <summary>
        /// 上傳頭像
        /// </summary>
        /// <param name="files">files</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> UploadPhoto(string memberID, IFormFile file)
        {
            if (string.IsNullOrEmpty(memberID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            if (file == null)
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "無檔案上傳."
                };
            }

            try
            {
                //this.logger.LogInformation($"UploadPhoto >>> {file.Length}");
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.UploadFilesService, "api/UploadFiles/Images", new FormFileCollection() { file });
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string photoUrl = (await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>()).FirstOrDefault();

                    ResponseResultDto responseResult = await this.EditData(new MemberInfoDto() { MemberID = memberID, Photo = photoUrl });
                    responseResult.Data = responseResult.Ok ? photoUrl : responseResult.Data;
                    return responseResult;
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Upload Photo Error >>> MemberID:{memberID} File:{file}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "上傳頭像發生錯誤."
                };
            }
        }
    }
}