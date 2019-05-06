using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Member.Command;
using GoBike.API.Service.Models.Member.Data;
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

        #region 會員資料

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="MemberInfoDto">memberInfo</param>
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
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/EditData", postData);
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
                    Data = "會員編輯發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="targetData">targetData</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetMemberInfo(string memberID, MemberBaseCommandDto targetData)
        {
            if (string.IsNullOrEmpty(memberID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            string postData = null;
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                //// 查詢會員本身資料
                if (targetData == null)
                {
                    postData = JsonConvert.SerializeObject(new MemberBaseCommandDto() { MemberID = memberID });
                    httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo", postData);
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

                //// 查詢指定會員資料
                postData = JsonConvert.SerializeObject(targetData);
                httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    MemberInfoDto targetMemberInfo = await httpResponseMessage.Content.ReadAsAsync<MemberInfoDto>();
                    if (targetMemberInfo == null)
                    {
                        return new ResponseResultDto()
                        {
                            Ok = false,
                            Data = "無會員資料."
                        };
                    }

                    if (memberID.Equals(targetData.MemberID))
                    {
                        return new ResponseResultDto()
                        {
                            Ok = false,
                            Data = "無法查詢會員本身資料."
                        };
                    }

                    postData = JsonConvert.SerializeObject(new MemberInteractiveCommandDto() { InitiatorID = memberID, ReceiverID = targetMemberInfo.MemberID });
                    httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Member/GetMemberInteractiveStatus", postData);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        targetMemberInfo.InteractiveStatus = await httpResponseMessage.Content.ReadAsAsync<int>();
                        return new ResponseResultDto()
                        {
                            Ok = true,
                            Data = targetMemberInfo
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
                this.logger.LogError($"Get Member Info Error >>> MemberID:{memberID} TargetData:{JsonConvert.SerializeObject(targetData)}\n{ex}");
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
                string postData = JsonConvert.SerializeObject(new MemberBaseCommandDto() { Email = email, Password = password });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Login", postData);
                string result = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = new MemberInfoDto()
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
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Register(MemberBaseCommandDto memberBaseCommand)
        {
            if (string.IsNullOrEmpty(memberBaseCommand.Email) || string.IsNullOrEmpty(memberBaseCommand.Password))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱或密碼無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(memberBaseCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Register", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Email:{memberBaseCommand.Email} Password:{memberBaseCommand.Password}\n{ex}");
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
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<ResponseResultDto> ResetPassword(MemberBaseCommandDto memberBaseCommand)
        {
            if (string.IsNullOrEmpty(memberBaseCommand.Email))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "信箱無效."
                };
            }

            try
            {
                memberBaseCommand.Password = Guid.NewGuid().ToString().Substring(0, 8);
                string postData = JsonConvert.SerializeObject(memberBaseCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/ResetPassword", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    EmailContext emailContext = EmailContext.GetResetPasswordEmailContext(memberBaseCommand.Email, memberBaseCommand.Password);
                    postData = JsonConvert.SerializeObject(emailContext);
                    httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.SmtpService, "api/SendEmail", postData);
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
                this.logger.LogError($"Reset Password Error >>> Email:{memberBaseCommand.Email}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "重設密碼發生錯誤."
                };
            }
        }

        /// <summary>
        /// 上傳頭像
        /// </summary>
        /// <param name="memberID">memberID</param>
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
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.UploadFilesService, "api/UploadFiles/Images", new FormFileCollection() { file });
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

        #endregion 會員資料

        #region 會員互動資料

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddBlacklist(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/AddBlacklist", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入黑名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddFriend(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/AddFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddFriendRequest(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/AddFriendRequest", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入好友請求發生錯誤."
                };
            }
        }

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteBlacklist(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/DeleteBlacklist", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除黑名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteFriend(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/DeleteFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteRequestForAddFriend(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/DeleteRequestForAddFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除加入好友請求發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetAddFriendRequestList(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/GetAddFriendRequestList", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
                Tuple<IEnumerable<MemberInfoDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs, (int)InteractiveStatusType.Request);
                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = getMemebrInfoListResult.Item2
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = getMemebrInfoListResult.Item1
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得加入好友請求名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetBlacklist(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/GetBlacklist", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
                Tuple<IEnumerable<MemberInfoDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs, (int)InteractiveStatusType.Black);
                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = getMemebrInfoListResult.Item2
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = getMemebrInfoListResult.Item1
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得黑名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetFriendList(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/GetFriendList", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
                Tuple<IEnumerable<MemberInfoDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs, (int)InteractiveStatusType.Friend);
                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = getMemebrInfoListResult.Item2
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = getMemebrInfoListResult.Item1
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得好友名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RejectBeFriend(MemberInteractiveCommandDto memberInteractiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/RejectBeFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "拒絕加入好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員資訊列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <param name="interactiveStatus">interactiveStatus</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        private async Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetMemebrInfoList(IEnumerable<string> memberIDs, int interactiveStatus)
        {
            if (memberIDs == null || memberIDs.Count() == 0)
            {
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(new List<MemberInfoDto>(), string.Empty);
            }

            try
            {
                string postData = JsonConvert.SerializeObject(memberIDs);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo/List", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    IEnumerable<MemberInfoDto> memberInfos = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberInfoDto>>();
                    foreach (MemberInfoDto memberInfo in memberInfos)
                    {
                        memberInfo.InteractiveStatus = interactiveStatus;
                    }

                    return Tuple.Create(memberInfos, string.Empty);
                }

                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, await httpResponseMessage.Content.ReadAsAsync<string>());
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Info List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)} InteractiveStatus:{interactiveStatus}\n{ex}");
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "取得會員資訊列表發生錯誤.");
            }
        }

        /// <summary>
        /// 驗證會員資料
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <param name="isVerifyInitiator">isVerifyInitiator</param>
        /// <param name="isVerifyReceiver">isVerifyReceiver</param>
        /// <returns>string</returns>
        private async Task<string> VerifyMemebr(MemberInteractiveCommandDto memberInteractiveCommand, bool isVerifyInitiator, bool isVerifyReceiver)
        {
            try
            {
                if (memberInteractiveCommand == null)
                {
                    return "會員互動指令資料不存在.";
                }

                List<string> verifyMemberIDList = new List<string>();
                if (isVerifyInitiator)
                {
                    if (string.IsNullOrEmpty(memberInteractiveCommand.InitiatorID))
                    {
                        return "發起者會員編號無效.";
                    }

                    if (memberInteractiveCommand.InitiatorID.Equals(memberInteractiveCommand.ReceiverID))
                    {
                        return "互動對象不得為會員本身.";
                    }

                    verifyMemberIDList.Add(memberInteractiveCommand.InitiatorID);
                }

                if (isVerifyReceiver)
                {
                    if (string.IsNullOrEmpty(memberInteractiveCommand.ReceiverID))
                    {
                        return "接收者會員編號無效.";
                    }

                    if (memberInteractiveCommand.ReceiverID.Equals(memberInteractiveCommand.InitiatorID))
                    {
                        return "互動對象不得為會員本身.";
                    }

                    verifyMemberIDList.Add(memberInteractiveCommand.ReceiverID);
                }

                if (verifyMemberIDList.Count == 0)
                {
                    return "無查詢資料.";
                }

                string postData = JsonConvert.SerializeObject(verifyMemberIDList);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/VerifyMember", postData);
                return httpResponseMessage.IsSuccessStatusCode ? string.Empty : "無會員資料.";
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Verify Memebr Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID} IsVerifyInitiator:{isVerifyInitiator} IsVerifyReceiver:{isVerifyReceiver}\n{ex}");
                return "驗證會員資料發生錯誤.";
            }
        }

        #endregion 會員互動資料
    }
}