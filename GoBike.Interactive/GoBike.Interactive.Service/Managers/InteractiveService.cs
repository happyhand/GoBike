using AutoMapper;
using GoBike.Interactive.Repository.Interface;
using GoBike.Interactive.Repository.Models;
using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Interactive.Service.Managers
{
    /// <summary>
    /// 互動服務
    /// </summary>
    public class InteractiveService : IInteractiveService
    {
        /// <summary>
        /// interactiveRepository
        /// </summary>
        private readonly IInteractiveRepository interactiveRepository;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<InteractiveService> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// memberRepository
        /// </summary>
        private readonly IMemberRepository memberRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveRepository">interactiveRepository</param>
        /// <param name="memberRepository">memberRepository</param>
        public InteractiveService(ILogger<InteractiveService> logger, IMapper mapper, IInteractiveRepository interactiveRepository, IMemberRepository memberRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.interactiveRepository = interactiveRepository;
            this.memberRepository = memberRepository;
        }

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> AddBlacklist(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string validInteractiveMemberDataResult = await this.ValidInteractiveMemberData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (!string.IsNullOrEmpty(validInteractiveMemberDataResult))
                {
                    return validInteractiveMemberDataResult;
                }

                //// 處理發起者互動資料
                bool isInitiatorSuccess = false;
                InteractiveData initiatorInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (initiatorInteractiveData == null)
                {
                    initiatorInteractiveData = this.CreateInteractiveData(interactiveInfo, (int)FriendStatusType.Black);
                    isInitiatorSuccess = await this.interactiveRepository.CreateInteractiveData(initiatorInteractiveData);
                }
                else
                {
                    if (initiatorInteractiveData.Status == (int)FriendStatusType.Black)
                    {
                        return this.GetInteractiveStatusMemo(initiatorInteractiveData, false);
                    }

                    initiatorInteractiveData.Status = (int)FriendStatusType.Black;
                    Tuple<bool, string> initiatorUpdateResult = await this.interactiveRepository.UpdateInteractiveData(initiatorInteractiveData);
                    isInitiatorSuccess = initiatorUpdateResult.Item1;
                    if (!isInitiatorSuccess)
                    {
                        this.logger.LogError($"Add Blacklist Fail >>> Data:{JsonConvert.SerializeObject(initiatorInteractiveData)}");
                        return initiatorUpdateResult.Item2;
                    }
                }

                //// 處理被動者互動資料
                bool isPassiveSuccess = true;
                InteractiveData passiveInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                if (passiveInteractiveData != null)
                {
                    if (passiveInteractiveData.Status != (int)FriendStatusType.Black)
                    {
                        isPassiveSuccess = await this.interactiveRepository.DeleteInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                    }
                }

                //// 檢測結果
                if (!isInitiatorSuccess || !isPassiveSuccess)
                {
                    this.logger.LogError($"Add Blacklist Fail >>> IsInitiatorSuccess:{isInitiatorSuccess} IsPassiveSuccess:{isPassiveSuccess}\nInitiatorInteractiveData:{JsonConvert.SerializeObject(initiatorInteractiveData)}\nPassiveInteractiveData:{JsonConvert.SerializeObject(passiveInteractiveData)}");
                    return "加入黑名單失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return "加入黑名單發生錯誤.";
            }
        }

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> AddFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string validInteractiveMemberDataResult = await this.ValidInteractiveMemberData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (!string.IsNullOrEmpty(validInteractiveMemberDataResult))
                {
                    return validInteractiveMemberDataResult;
                }

                //// 處理被動者互動資料
                bool isPassiveSuccess = false;
                InteractiveData passiveInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                if (passiveInteractiveData == null)
                {
                    return "無請求者的互動資料.";
                }

                switch (passiveInteractiveData.Status)
                {
                    case (int)FriendStatusType.Black:
                        return this.GetInteractiveStatusMemo(passiveInteractiveData, true);

                    case (int)FriendStatusType.Request:
                        passiveInteractiveData.Status = (int)FriendStatusType.Friend;
                        Tuple<bool, string> passiveUpdateResult = await this.interactiveRepository.UpdateInteractiveData(passiveInteractiveData);
                        isPassiveSuccess = passiveUpdateResult.Item1;
                        if (!isPassiveSuccess)
                        {
                            this.logger.LogError($"Add Friend Fail >>> Passive Data:{JsonConvert.SerializeObject(passiveInteractiveData)}");
                            return passiveUpdateResult.Item2;
                        }
                        break;

                    case (int)FriendStatusType.Friend:
                        isPassiveSuccess = true;
                        break;
                }

                //// 處理發起者互動資料
                bool isInitiatorSuccess = false;
                InteractiveData initiatorInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (initiatorInteractiveData == null)
                {
                    initiatorInteractiveData = this.CreateInteractiveData(interactiveInfo, (int)FriendStatusType.Friend);
                    isInitiatorSuccess = await this.interactiveRepository.CreateInteractiveData(initiatorInteractiveData);
                }
                else
                {
                    switch (initiatorInteractiveData.Status)
                    {
                        case (int)FriendStatusType.Black:
                            return this.GetInteractiveStatusMemo(passiveInteractiveData, false);

                        case (int)FriendStatusType.Request:
                            initiatorInteractiveData.Status = (int)FriendStatusType.Friend;
                            Tuple<bool, string> initiatorUpdateResult = await this.interactiveRepository.UpdateInteractiveData(initiatorInteractiveData);
                            isInitiatorSuccess = initiatorUpdateResult.Item1;
                            if (!isInitiatorSuccess)
                            {
                                this.logger.LogError($"Add Friend Fail >>> Initiator Data:{JsonConvert.SerializeObject(initiatorInteractiveData)}");
                                return initiatorUpdateResult.Item2;
                            }

                            break;

                        case (int)FriendStatusType.Friend:
                            isInitiatorSuccess = true;
                            break;
                    }
                }

                //// 檢測結果
                if (!isInitiatorSuccess || !isPassiveSuccess)
                {
                    this.logger.LogError($"Add Friend Fail >>> IsInitiatorSuccess:{isInitiatorSuccess} IsPassiveSuccess:{isPassiveSuccess}\nInitiatorInteractiveData:{JsonConvert.SerializeObject(initiatorInteractiveData)}\nPassiveInteractiveData:{JsonConvert.SerializeObject(passiveInteractiveData)}");
                    return "加入好友失敗.";
                }

                //// TODO 連線 Server 發送即時通知

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return "加入好友發生錯誤.";
            }
        }

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> AddFriendRequest(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string validInteractiveMemberDataResult = await this.ValidInteractiveMemberData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (!string.IsNullOrEmpty(validInteractiveMemberDataResult))
                {
                    return validInteractiveMemberDataResult;
                }

                string initiatorInteractiveStatusMemo = await this.GetInteractiveStatusMemo(interactiveInfo, false);
                if (!string.IsNullOrEmpty(initiatorInteractiveStatusMemo))
                {
                    return initiatorInteractiveStatusMemo;
                }

                string passiveInteractiveStatusMemo = await this.GetInteractiveStatusMemo(interactiveInfo, true);
                if (!string.IsNullOrEmpty(passiveInteractiveStatusMemo))
                {
                    return passiveInteractiveStatusMemo;
                }

                InteractiveData interactiveData = this.CreateInteractiveData(interactiveInfo, (int)FriendStatusType.Request);
                bool isSuccess = await this.interactiveRepository.CreateInteractiveData(interactiveData);
                if (!isSuccess)
                {
                    this.logger.LogError($"Add Friend Request Fail >>> InteractiveData:{JsonConvert.SerializeObject(interactiveData)}");
                    return "建立互動資料失敗.";
                }

                //// TODO 連線 Server 發送即時通知

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return "加入好友請求發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> DeleteBlacklist(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
                {
                    return "會員編號無效.";
                }

                InteractiveData initiatorInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (initiatorInteractiveData == null)
                {
                    return "無互動資料.";
                }

                if (initiatorInteractiveData.Status != (int)FriendStatusType.Black)
                {
                    return "未設定對方為黑名單.";
                }

                bool isSuccess = await this.interactiveRepository.DeleteInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (!isSuccess)
                {
                    this.logger.LogError($"Delete Blacklist Fail >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}");
                    return "刪除黑名單失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return "刪除黑名單發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> DeleteFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
                {
                    return "會員編號無效.";
                }

                //// 處理發起者互動資料
                InteractiveData initiatorInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                bool isInitiatorSuccess = true;
                if (initiatorInteractiveData != null)
                {
                    if (initiatorInteractiveData.Status == (int)FriendStatusType.Black)
                    {
                        return this.GetInteractiveStatusMemo(initiatorInteractiveData, false);
                    }

                    isInitiatorSuccess = await this.interactiveRepository.DeleteInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                }

                //// 處理被動者互動資料
                InteractiveData passiveInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                bool isPassiveSuccess = true;
                if (passiveInteractiveData != null)
                {
                    if (passiveInteractiveData.Status == (int)FriendStatusType.Black)
                    {
                        return this.GetInteractiveStatusMemo(passiveInteractiveData, true);
                    }

                    isPassiveSuccess = await this.interactiveRepository.DeleteInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                }

                //// 檢測結果
                if (!isInitiatorSuccess || !isPassiveSuccess)
                {
                    this.logger.LogError($"Delete Friend Fail >>> IsInitiatorSuccess:{isInitiatorSuccess} IsPassiveSuccess:{isPassiveSuccess}\nInitiatorInteractiveData:{JsonConvert.SerializeObject(initiatorInteractiveData)}\nPassiveInteractiveData:{JsonConvert.SerializeObject(passiveInteractiveData)}");
                    return "刪除好友失敗.";
                }

                //// TODO 連線 Server 發送即時通知

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return "刪除好友錯誤.";
            }
        }

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> DeleteRequestForAddFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
                {
                    return "會員編號無效.";
                }

                InteractiveData initiatorInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (initiatorInteractiveData == null)
                {
                    return "無互動資料.";
                }

                if (initiatorInteractiveData.Status != (int)FriendStatusType.Request)
                {
                    return this.GetInteractiveStatusMemo(initiatorInteractiveData, false);
                }

                bool isSuccess = await this.interactiveRepository.DeleteInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                if (!isSuccess)
                {
                    this.logger.LogError($"Delete Request For Add Friend Fail >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}");
                    return "刪除加入好友請求失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return "刪除加入好友請求發生錯誤.";
            }
        }

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetAddFriendRequestList(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID))
                {
                    return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<InteractiveData> interactiveDatas = await this.interactiveRepository.GetAddFriendRequestList(interactiveInfo.InitiatorID);
                IEnumerable<MemberData> memberDatas = await this.memberRepository.GetMemebrDataList(interactiveDatas.Select(x => x.InitiatorID));
                return Tuple.Create(this.TransformMemberDataToInfo(memberDatas, (int)FriendStatusType.Request), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "取得加入好友請求名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetBlacklist(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID))
                {
                    return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<InteractiveData> interactiveDatas = await this.interactiveRepository.GetBlacklist(interactiveInfo.InitiatorID);
                IEnumerable<MemberData> memberDatas = await this.memberRepository.GetMemebrDataList(interactiveDatas.Select(x => x.PassiveID));
                return Tuple.Create(this.TransformMemberDataToInfo(memberDatas, (int)FriendStatusType.Black), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "取得黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetFriendList(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID))
                {
                    return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<InteractiveData> interactiveDatas = await this.interactiveRepository.GetFriendList(interactiveInfo.InitiatorID);
                IEnumerable<MemberData> memberDatas = await this.memberRepository.GetMemebrDataList(interactiveDatas.Select(x => x.PassiveID));
                return Tuple.Create(this.TransformMemberDataToInfo(memberDatas, (int)FriendStatusType.Friend), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "取得好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> RejectBeFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
                {
                    return "會員編號無效.";
                }

                InteractiveData passiveInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                if (passiveInteractiveData == null)
                {
                    return "無請求者的互動資料.";
                }

                if (passiveInteractiveData.Status != (int)FriendStatusType.Request)
                {
                    return this.GetInteractiveStatusMemo(passiveInteractiveData, true);
                }

                bool isSuccess = await this.interactiveRepository.DeleteInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                if (!isSuccess)
                {
                    this.logger.LogError($"Reject Be Friend Fail >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}");
                    return "拒絕加入好友失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return "拒絕加入好友發生錯誤.";
            }
        }

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        public async Task<Tuple<MemberInfoDto, string>> SearchFriend(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
                {
                    return Tuple.Create<MemberInfoDto, string>(null, "會員編號無效.");
                }

                InteractiveData passiveInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID);
                if (passiveInteractiveData != null)
                {
                    if (passiveInteractiveData.Status == (int)FriendStatusType.Black)
                    {
                        return Tuple.Create<MemberInfoDto, string>(null, this.GetInteractiveStatusMemo(passiveInteractiveData, true));
                    }
                }

                InteractiveData initiatorInteractiveData = await this.interactiveRepository.GetInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
                MemberData memberData = await this.memberRepository.GetMemebrData(interactiveInfo.PassiveID);
                MemberInfoDto memberInfo = this.mapper.Map<MemberInfoDto>(memberData);
                if (initiatorInteractiveData == null)
                {
                    if (passiveInteractiveData != null && passiveInteractiveData.Status == (int)FriendStatusType.Request)
                    {
                        memberInfo.Status = (int)FriendStatusType.RequestHandler;
                    }
                    else
                    {
                        memberInfo.Status = (int)FriendStatusType.None;
                    }
                }
                else
                {
                    memberInfo.Status = initiatorInteractiveData.Status;
                }

                return Tuple.Create(memberInfo, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return Tuple.Create<MemberInfoDto, string>(null, "搜尋好友發生錯誤.");
            }
        }

        /// <summary>
        /// 創建互動資料
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <param name="status">status</param>
        /// <returns>FriendData</returns>
        private InteractiveData CreateInteractiveData(InteractiveInfoDto interactiveInfo, int status)
        {
            return new InteractiveData()
            {
                InitiatorID = interactiveInfo.InitiatorID,
                PassiveID = interactiveInfo.PassiveID,
                Status = status
            };
        }

        /// <summary>
        /// 取得互動資料狀態備註
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <param name="isReverse">isReverse</param>
        /// <returns>string</returns>
        private async Task<string> GetInteractiveStatusMemo(InteractiveInfoDto interactiveInfo, bool isReverse)
        {
            InteractiveData interactiveData = isReverse ? await this.interactiveRepository.GetInteractiveData(interactiveInfo.PassiveID, interactiveInfo.InitiatorID) : await this.interactiveRepository.GetInteractiveData(interactiveInfo.InitiatorID, interactiveInfo.PassiveID);
            return this.GetInteractiveStatusMemo(interactiveData, isReverse);
        }

        /// <summary>
        /// 取得互動資料狀態備註
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <param name="isReverse">isReverse</param>
        /// <returns>string</returns>
        private string GetInteractiveStatusMemo(InteractiveData interactiveData, bool isReverse)
        {
            if (interactiveData == null)
            {
                return string.Empty;
            }

            switch (interactiveData.Status)
            {
                case (int)FriendStatusType.Black:
                    return isReverse ? "對方已設該會員為黑名單." : "對方已設為黑名單.";

                case (int)FriendStatusType.Request:
                    return isReverse ? "對方已發送加入好友請求." : "已發送加入好友請求.";

                case (int)FriendStatusType.Friend:
                    return isReverse ? "對方已與該會員為好友關係." : "已為好友關係.";

                default:
                    return $"無法辨別此互動狀態:{interactiveData.Status}.";
            }
        }

        /// <summary>
        /// 轉換會員資料
        /// </summary>
        /// <param name="memberDatas">memberDatas</param>
        /// <param name="status">status</param>
        /// <returns>MemberInfoDtos</returns>
        private IEnumerable<MemberInfoDto> TransformMemberDataToInfo(IEnumerable<MemberData> memberDatas, int status)
        {
            IEnumerable<MemberInfoDto> memberInfos = this.mapper.Map<IEnumerable<MemberInfoDto>>(memberDatas);
            foreach (MemberInfoDto memberInfo in memberInfos)
            {
                memberInfo.Status = status;
            }

            return memberInfos;
        }

        /// <summary>
        /// 驗證互動會員資料
        /// </summary>
        /// <param name="initiatorID">initiatorID</param>
        /// <param name="passiveID">passiveID</param>
        /// <returns>string</returns>
        private async Task<string> ValidInteractiveMemberData(string initiatorID, string passiveID)
        {
            if (string.IsNullOrEmpty(initiatorID) || string.IsNullOrEmpty(passiveID))
            {
                return "會員編號無效.";
            }

            bool missInitiator = await this.memberRepository.GetMemebrData(initiatorID) == null;
            bool missPassive = await this.memberRepository.GetMemebrData(passiveID) == null;
            if (missInitiator || missPassive)
            {
                return "會員不存在.";
            }

            return string.Empty;
        }
    }
}