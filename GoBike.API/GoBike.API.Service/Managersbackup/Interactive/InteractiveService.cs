//using AutoMapper;
//using GoBike.API.Core.Applibs;
//using GoBike.API.Core.Resource;
//using GoBike.API.Service.Interface.Interactive;
//using GoBike.API.Service.Models.Interactive.Command;
//using GoBike.API.Service.Models.Member.View;
//using GoBike.API.Service.Models.Response;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace GoBike.API.Service.Managers.Interactive
//{
//    /// <summary>
//    /// 互動服務
//    /// </summary>
//    public class InteractiveService : IInteractiveService
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<InteractiveService> logger;

//        /// <summary>
//        /// mapper
//        /// </summary>
//        private readonly IMapper mapper;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="mapper">mapper</param>
//        public InteractiveService(ILogger<InteractiveService> logger, IMapper mapper)
//        {
//            this.logger = logger;
//            this.mapper = mapper;
//        }

//        /// <summary>
//        /// 加入黑名單
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> AddBlacklist(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                bool verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
//                if (!verifyMemberResult)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = "驗證會員資料失敗."
//                    };
//                }

//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/Add", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "加入黑名單發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 加入好友
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> AddFriend(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                bool verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
//                if (!verifyMemberResult)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = "驗證會員資料失敗."
//                    };
//                }

//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/Add", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Add Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "加入好友發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 加入好友請求
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> AddFriendRequest(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                bool verifyMemberResult = await this.VerifyMemebr(memberInteractiveCommand, true, true);
//                if (!verifyMemberResult)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = "驗證會員資料失敗."
//                    };
//                }

//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/FriendRequest/Add", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "加入好友請求發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 刪除黑名單
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> DeleteBlacklist(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/Delete", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "刪除黑名單發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 刪除好友
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> DeleteFriend(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/Delete", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "刪除好友發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 刪除加入好友請求
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> DeleteRequestForAddFriend(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/FriendRequest/Delete", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "刪除加入好友請求發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 取得加入好友請求名單
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> GetAddFriendRequestList(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/FriendRequest/Get", postData);
//                if (!httpResponseMessage.IsSuccessStatusCode)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                    };
//                }

//                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
//                Tuple<IEnumerable<MemberSimpleInfoViewDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs);
//                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = getMemebrInfoListResult.Item2
//                    };
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = true,
//                    Data = getMemebrInfoListResult.Item1
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "取得加入好友請求名單發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 取得黑名單
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> GetBlacklist(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/Get", postData);
//                if (!httpResponseMessage.IsSuccessStatusCode)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                    };
//                }

//                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
//                Tuple<IEnumerable<MemberSimpleInfoViewDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs);
//                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = getMemebrInfoListResult.Item2
//                    };
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = true,
//                    Data = getMemebrInfoListResult.Item1
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "取得黑名單發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 取得好友名單
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> GetFriendList(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/Get", postData);
//                if (!httpResponseMessage.IsSuccessStatusCode)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                    };
//                }

//                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
//                Tuple<IEnumerable<MemberSimpleInfoViewDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs);
//                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = getMemebrInfoListResult.Item2
//                    };
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = true,
//                    Data = getMemebrInfoListResult.Item1
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "取得好友名單發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 拒絕加入好友
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> RejectBeFriend(MemberInteractiveCommandDto memberInteractiveCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInteractiveCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/Reject", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "拒絕加入好友發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 取得會員資訊列表
//        /// </summary>
//        /// <param name="memberIDs">memberIDs</param>
//        /// <returns>Tuple(MemberInteractiveInfoViewDtos, string)</returns>
//        private async Task<Tuple<IEnumerable<MemberSimpleInfoViewDto>, string>> GetMemebrInfoList(IEnumerable<string> memberIDs)
//        {
//            if (memberIDs == null || !memberIDs.Any())
//            {
//                return Tuple.Create<IEnumerable<MemberSimpleInfoViewDto>, string>(new List<MemberSimpleInfoViewDto>(), string.Empty);
//            }

//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberIDs);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo/List", postData);
//                if (httpResponseMessage.IsSuccessStatusCode)
//                {
//                    IEnumerable<MemberSimpleInfoViewDto> memberSimpleInfoViews = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberSimpleInfoViewDto>>();
//                    return Tuple.Create(memberSimpleInfoViews, string.Empty);
//                }

//                string failResult = await httpResponseMessage.Content.ReadAsAsync<string>();
//                return Tuple.Create<IEnumerable<MemberSimpleInfoViewDto>, string>(null, failResult);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get Memebr Info List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
//                return Tuple.Create<IEnumerable<MemberSimpleInfoViewDto>, string>(null, "取得會員資訊列表發生錯誤.");
//            }
//        }

//        /// <summary>
//        /// 驗證會員資料
//        /// </summary>
//        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
//        /// <param name="isVerifyInitiator">isVerifyInitiator</param>
//        /// <param name="isVerifyReceiver">isVerifyReceiver</param>
//        /// <returns>bool</returns>
//        private async Task<bool> VerifyMemebr(MemberInteractiveCommandDto memberInteractiveCommand, bool isVerifyInitiator, bool isVerifyReceiver)
//        {
//            try
//            {
//                if (memberInteractiveCommand == null)
//                {
//                    return false;
//                }

//                List<string> verifyMemberIDList = new List<string>();
//                if (isVerifyInitiator)
//                {
//                    if (string.IsNullOrEmpty(memberInteractiveCommand.InitiatorID))
//                    {
//                        return false;
//                    }

//                    if (memberInteractiveCommand.InitiatorID.Equals(memberInteractiveCommand.ReceiverID))
//                    {
//                        this.logger.LogError($"Verify Memebr Fail >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n");
//                        return false;
//                    }

//                    verifyMemberIDList.Add(memberInteractiveCommand.InitiatorID);
//                }

//                if (isVerifyReceiver)
//                {
//                    if (string.IsNullOrEmpty(memberInteractiveCommand.ReceiverID))
//                    {
//                        return false;
//                    }

//                    if (memberInteractiveCommand.ReceiverID.Equals(memberInteractiveCommand.InitiatorID))
//                    {
//                        this.logger.LogError($"Verify Memebr Fail >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID}\n");
//                        return false;
//                    }

//                    verifyMemberIDList.Add(memberInteractiveCommand.ReceiverID);
//                }

//                if (!verifyMemberIDList.Any())
//                {
//                    return false;
//                }

//                string postData = JsonConvert.SerializeObject(verifyMemberIDList);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/VerifyMember", postData);
//                return httpResponseMessage.IsSuccessStatusCode;
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Verify Memebr Error >>> InitiatorID:{memberInteractiveCommand.InitiatorID} ReceiverID:{memberInteractiveCommand.ReceiverID} IsVerifyInitiator:{isVerifyInitiator} IsVerifyReceiver:{isVerifyReceiver}\n{ex}");
//                return false;
//            }
//        }
//    }
//}