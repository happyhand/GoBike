using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Core.Resource.Enum;
using GoBike.API.Repository.Interface;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Member.View;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// redisRepository
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="redisRepository">redisRepository</param>
        public MemberService(ILogger<MemberService> logger, IMapper mapper, IRedisRepository redisRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.redisRepository = redisRepository;
        }

        #region 註冊\登入

        /// <summary>
        /// 刪除會員 Session ID
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="sessionID">sessionID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteSessionID(string memberID, string sessionID)
        {
            try
            {
                string cacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.Session}-{sessionID}-{memberID}";
                bool result = await this.redisRepository.DeleteCache(cacheKey);
                if (result)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = "刪除會員 Session ID 成功."
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除會員 Session ID 失敗."
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Session ID Error >>> MemberID:{memberID} SessionID:{sessionID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除會員 Session ID 發生錯誤."
                };
            }
        }

        /// <summary>
        /// 延長會員 Session ID 期限
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="sessionID">sessionID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> ExtendSessionIDExpire(string memberID, string sessionID)
        {
            try
            {
                string cacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.Session}-{sessionID}-{memberID}";
                bool result = await this.redisRepository.UpdateCacheExpire(cacheKey, TimeSpan.FromMinutes(AppSettingHelper.Appsetting.SeesionDeadline));
                if (result)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = "延長 Session ID 成功."
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "延長 Session ID 失敗."
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Extend Session ID Expire Error >>> MemberID:{memberID} SessionID:{sessionID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "延長 Session ID 發生錯誤."
                };
            }
        }

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Login(string email, string password)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { Email = email, Password = password });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Login", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string memberID = await httpResponseMessage.Content.ReadAsAsync<string>();
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = new string[] { memberID, this.CreateLoginToken(email, password, string.Empty, string.Empty) }
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
            try
            {
                string[] dataArr = token.Split(CommonFlagHelper.CommonFlag.SeparateFlag);
                string data1 = Utility.DecryptAES(dataArr[0]);
                string data2 = Utility.DecryptAES(dataArr[1]);
                if (data1.Equals(CommonFlagHelper.CommonFlag.PlatformFlag.FB))
                {
                    string fbToken = Utility.DecryptAES(dataArr[2]);
                    return await this.LoginFB(data2, fbToken);
                }

                if (data1.Equals(CommonFlagHelper.CommonFlag.PlatformFlag.Google))
                {
                    string googleToken = Utility.DecryptAES(dataArr[2]);
                    return await this.LoginGoogle(data2, googleToken);
                }

                return await this.Login(data1, data2);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Auto Token Error >>> Token:{token}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員登入發生錯誤."
                };
            }
        }

        /// <summary>
        /// 會員登入 (FB)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> LoginFB(string email, string token)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { Email = email, FBToken = token });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Login/FB", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string memberID = await httpResponseMessage.Content.ReadAsAsync<string>();
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = new string[] { memberID, this.CreateLoginToken(email, string.Empty, token, string.Empty) }
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
                this.logger.LogError($"Login FB Error >>> FBToken:{token}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員登入發生錯誤."
                };
            }
        }

        /// <summary>
        /// 會員登入 (Google)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> LoginGoogle(string email, string token)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { Email = email, GoogleToken = token });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Login/Google", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string memberID = await httpResponseMessage.Content.ReadAsAsync<string>();
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = new string[] { memberID, this.CreateLoginToken(email, string.Empty, string.Empty, token) }
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
                this.logger.LogError($"Login Google Error >>> GoogleToken:{token}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員登入發生錯誤."
                };
            }
        }

        /// <summary>
        /// 紀錄會員 Session ID
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="sessionID">sessionID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RecordSessionID(string memberID, string sessionID)
        {
            try
            {
                string cacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.Session}-{sessionID}-{memberID}";
                bool result = await this.redisRepository.SetCache(cacheKey, memberID, TimeSpan.FromMinutes(AppSettingHelper.Appsetting.SeesionDeadline));
                if (result)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = "紀錄會員 Session ID 成功."
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "紀錄會員 Session ID 失敗."
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Record Session ID Error >>> MemberID:{memberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "紀錄會員 Session ID 發生錯誤."
                };
            }
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Register(string email, string password)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { Email = email, Password = password });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Register", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
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
        /// 建立登入 Token
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <param name="fbToken">fbToken</param>
        /// <param name="googleToken">googleToken</param>
        /// <returns>string</returns>
        private string CreateLoginToken(string email, string password, string fbToken, string googleToken)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                return $"{Utility.EncryptAES(email)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(password)}";
            }

            if (!string.IsNullOrEmpty(fbToken))
            {
                return $"{Utility.EncryptAES(CommonFlagHelper.CommonFlag.PlatformFlag.FB)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(email)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(fbToken)}";
            }

            if (!string.IsNullOrEmpty(googleToken))
            {
                return $"{Utility.EncryptAES(CommonFlagHelper.CommonFlag.PlatformFlag.Google)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(email)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(googleToken)}";
            }

            return string.Empty;
        }

        #endregion 註冊\登入

        #region 會員資料

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> EditData(MemberDto memberDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(memberDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/EditData", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(memberDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編輯發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員設定資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetSettingData(string memberID)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { MemberID = memberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/SearchMember", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    MemberSettingViewDto memberSettingViewDto = await httpResponseMessage.Content.ReadAsAsync<MemberSettingViewDto>();
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = memberSettingViewDto
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
                this.logger.LogError($"Get Setting Data Error >>> MemberID:{memberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得會員設定資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 會員重設密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<ResponseResultDto> ResetPassword(string email)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { Email = email });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/ResetPassword", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string password = await httpResponseMessage.Content.ReadAsAsync<string>();
                    EmailContext emailContext = EmailContext.GetResetPasswordEmailContext(email, password);
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
                this.logger.LogError($"Reset Password Error >>> Email:{email}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員重設密碼發生錯誤."
                };
            }
        }

        /// <summary>
        /// 搜尋會員
        /// </summary>
        /// <param name="searchKey">searchKey</param>
        /// <param name="searcher">searcher</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SearchMember(string searchKey, string searcher)
        {
            try
            {
                MemberDto memberDto = new MemberDto();
                //// 判斷 Search Key
                if (searchKey.Contains("@"))
                {
                    memberDto.Email = searchKey;
                }
                else if (searchKey.Length == 6) //// 目前只能先寫死，待思考有沒有其他更好的方式
                {
                    memberDto.MemberID = searchKey;
                }
                else
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = "查詢參數無效."
                    };
                }

                string postData = JsonConvert.SerializeObject(memberDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/SearchMember", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    MemberDetailInfoViewDto searchMemberDto = await httpResponseMessage.Content.ReadAsAsync<MemberDetailInfoViewDto>();
                    if (!string.IsNullOrEmpty(searcher) && searcher.Equals(searchMemberDto.MemberID))
                    {
                        return new ResponseResultDto()
                        {
                            Ok = false,
                            Data = "無法查詢會員本身資料."
                        };
                    }

                    string fuzzyCacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.Session}-*-{searchMemberDto.MemberID}";
                    string cacheKey = this.redisRepository.GetRedisKeys(fuzzyCacheKey).FirstOrDefault();
                    searchMemberDto.OnlineType = string.IsNullOrEmpty(cacheKey) ? (int)OnlineStatusType.Offline : (int)OnlineStatusType.Online;

                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = searchMemberDto
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
                this.logger.LogError($"Search Member Error >>> SearchKey:{searchKey}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "搜尋會員發生錯誤."
                };
            }
        }

        #endregion 會員資料

        #region 騎乘資料

        /// <summary>
        /// 新增騎乘資料
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddRideData(RideDto rideDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(rideDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Ride/Add", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Ride Data Error >>> Data:{JsonConvert.SerializeObject(rideDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "新增騎乘資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得騎乘資料
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetRideData(RideDto rideDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(rideDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Ride/Get", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<RideDetailInfoViewDto>()
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
                this.logger.LogError($"Get Ride Data Error >>> RideID:{rideDto.RideID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得騎乘資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員的騎乘資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetRideDataListOfMember(string memberID)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { MemberID = memberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Ride/GetList", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<RideDetailInfoViewDto>>()
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
                this.logger.LogError($"Get Ride Data List Of Member Error >>> MemberID:{memberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得會員的騎乘資料列表發生錯誤."
                };
            }
        }

        #endregion 騎乘資料

        #region 互動資料

        /// <summary>
        /// 取得被加入好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetBeAddFriendList(string memberID)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { MemberID = memberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/GetBeAddFriendList", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberSimpleInfoViewDto>>()
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
                this.logger.LogError($"Get Be Add Friend List Error >>> MemberID:{memberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得被加入好友名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetBlackList(string memberID)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { MemberID = memberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/GetBlackList", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberSimpleInfoViewDto>>()
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
                this.logger.LogError($"Get Black List Error >>> MemberID:{memberID}\n{ex}");
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
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetFriendList(string memberID)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new MemberDto() { MemberID = memberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/GetFriendList", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberSimpleInfoViewDto>>()
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
                this.logger.LogError($"Get Friend List Error >>> MemberID:{memberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得好友名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> JoinBlack(InteractiveDto interactiveDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(interactiveDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/JoinBlack", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Black Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
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
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> JoinFriend(InteractiveDto interactiveDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(interactiveDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/JoinFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Friend Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 移除黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RemoveBlack(InteractiveDto interactiveDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(interactiveDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/RemoveBlack", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Black Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "移除黑名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RemoveFriend(InteractiveDto interactiveDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(interactiveDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/RemoveFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Friend Error >>> MemberID:{interactiveDto.MemberID} InteractiveID:{interactiveDto.InteractiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "移除好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SearchFriend(InteractiveDto interactiveDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(interactiveDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/Friendship/SearchFriend", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<MemberSimpleInfoViewDto>()
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
                this.logger.LogError($"Search Friend Error >>> MemberID:{interactiveDto.MemberID} SearchKey:{interactiveDto.SearchKey}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "搜尋好友發生錯誤."
                };
            }
        }

        #endregion 互動資料
    }
}