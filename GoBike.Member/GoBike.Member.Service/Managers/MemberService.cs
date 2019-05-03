using AutoMapper;
using GoBike.Member.Core.Resource;
using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoBike.Member.Service.Managers
{
    /// <summary>
    /// 會員服務
    /// </summary>
    public class MemberService : IMemberService
    {
        /// <summary>
        /// bikeRepository
        /// </summary>
        private readonly IBikeRepository bikeRepository;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<MemberService> logger;

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
        /// <param name="mapper">mapper</param>
        /// <param name="memberRepository">memberRepository</param>
        /// <param name="bikeRepository">bikeRepository</param>
        public MemberService(ILogger<MemberService> logger, IMapper mapper, IMemberRepository memberRepository, IBikeRepository bikeRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberRepository = memberRepository;
            this.bikeRepository = bikeRepository;
        }

        #region 會員資料

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <param name="IsStrictPassword">IsStrictPassword</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        public async Task<Tuple<MemberInfoDto, string>> EditData(MemberInfoDto memberInfo, bool IsStrictPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(memberInfo.MemberID))
                {
                    return Tuple.Create<MemberInfoDto, string>(null, "會員編號無效.");
                }

                MemberData memberData = await this.memberRepository.GetMemebrDataByMemberID(memberInfo.MemberID);
                if (memberData == null)
                {
                    return Tuple.Create<MemberInfoDto, string>(null, "無會員資料.");
                }

                string updateMemberDataHandlerResult = this.UpdateMemberDataHandler(memberInfo, memberData, IsStrictPassword);
                if (!string.IsNullOrEmpty(updateMemberDataHandlerResult))
                {
                    return Tuple.Create<MemberInfoDto, string>(null, updateMemberDataHandlerResult);
                }

                Tuple<bool, string> result = await this.memberRepository.UpdateMemebrData(memberData);
                if (!result.Item1)
                {
                    this.logger.LogError($"Edit Data Fail For Update Member Data >>> Data:{JsonConvert.SerializeObject(memberData)}");
                    return Tuple.Create<MemberInfoDto, string>(null, result.Item2);
                }

                return Tuple.Create(this.mapper.Map<MemberInfoDto>(memberData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
                return Tuple.Create<MemberInfoDto, string>(null, "會員編輯發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        public async Task<Tuple<MemberInfoDto, string>> GetMemberInfo(MemberInfoDto memberInfo)
        {
            try
            {
                MemberData memberData = null;
                if (!string.IsNullOrEmpty(memberInfo.MemberID))
                {
                    memberData = await this.memberRepository.GetMemebrDataByMemberID(memberInfo.MemberID);
                }
                else if (!string.IsNullOrEmpty(memberInfo.Email))
                {
                    memberData = await this.memberRepository.GetMemebrDataByEmail(memberInfo.Email);
                }
                else if (!string.IsNullOrEmpty(memberInfo.Mobile))
                {
                    memberData = await this.memberRepository.GetMemebrDataByMobile(memberInfo.Mobile);
                }
                else
                {
                    return Tuple.Create<MemberInfoDto, string>(null, "無效的查詢參數.");
                }

                if (memberData == null)
                {
                    return Tuple.Create<MemberInfoDto, string>(null, "無會員資料.");
                }

                return Tuple.Create(this.mapper.Map<MemberInfoDto>(memberData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info Error >>> MemberID:{memberInfo.MemberID} Email:{memberInfo.Email} Mobile:{memberInfo.Mobile}\n{ex}");
                return Tuple.Create<MemberInfoDto, string>(null, "取得會員資訊發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員資訊列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetMemberInfoList(IEnumerable<string> memberIDs)
        {
            try
            {
                if (memberIDs == null || memberIDs.Count() == 0)
                {
                    return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "無會員編號列表.");
                }

                IEnumerable<MemberData> memberDatas = await this.memberRepository.GetMemebrDataList(memberIDs);
                return Tuple.Create(this.mapper.Map<IEnumerable<MemberInfoDto>>(memberDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "取得會員資訊列表發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>Tuple(string, string)</returns>
        public async Task<Tuple<string, string>> Login(MemberInfoDto memberInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(memberInfo.Email) || string.IsNullOrEmpty(memberInfo.Password))
                {
                    return Tuple.Create(string.Empty, "信箱或密碼無效.");
                }

                MemberData memberData = await this.memberRepository.GetMemebrDataByEmail(memberInfo.Email);
                if (memberData == null)
                {
                    return Tuple.Create(string.Empty, "無會員資料.");
                }

                if (!Utility.DecryptAES(memberData.Password).Equals(memberInfo.Password))
                {
                    return Tuple.Create(string.Empty, "密碼驗證失敗.");
                }

                return Tuple.Create(memberData.MemberID, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Error >>> Email:{memberInfo.Email} Password:{memberInfo.Password}\n{ex}");
                return Tuple.Create(string.Empty, "會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>string</returns>
        public async Task<string> Register(MemberInfoDto memberInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(memberInfo.Email) || string.IsNullOrEmpty(memberInfo.Password))
                {
                    return "信箱或密碼無效.";
                }

                if (!Utility.IsValidEmail(memberInfo.Email))
                {
                    return "信箱格式錯誤.";
                }

                if (!this.IsValidPassword(memberInfo.Password))
                {
                    return "密碼格式錯誤.";
                }

                bool emailHasRegister = await this.memberRepository.GetMemebrDataByEmail(memberInfo.Email) != null;
                if (emailHasRegister)
                {
                    return "此信箱已經被註冊.";
                }

                Tuple<long, string> getMemberSerialNumberResult = await this.memberRepository.GetMemberSerialNumber();
                if (!string.IsNullOrEmpty(getMemberSerialNumberResult.Item2))
                {
                    return getMemberSerialNumberResult.Item2;
                }

                MemberData memberData = new MemberData()
                {
                    MemberID = getMemberSerialNumberResult.Item1.ToString(),
                    Email = memberInfo.Email,
                    Password = Utility.EncryptAES(memberInfo.Password),
                    CreateDate = DateTime.Now,
                };

                bool isSuccess = await this.memberRepository.CreateMemberData(memberData);
                if (!isSuccess)
                {
                    this.logger.LogError($"Register Fail For Create Member Data >>> Data:{JsonConvert.SerializeObject(memberData)}");
                    return "會員註冊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
                return "會員註冊發生錯誤.";
            }
        }

        /// <summary>
        /// 驗證會員資料
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>string</returns>
        public async Task<string> VerifyMemberList(IEnumerable<string> memberIDs)
        {
            try
            {
                if (memberIDs == null || memberIDs.Count() == 0)
                {
                    return "無查詢資料.";
                }

                bool result = await this.memberRepository.VerifyMemberList(memberIDs.Distinct());
                return result ? string.Empty : "無會員資料.";
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Verify Member List Error >>> Data:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return "驗證會員資料發生錯誤.";
            }
        }

        /// <summary>
        /// 驗證密碼格式
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        private bool IsValidPassword(string password)
        {
            int passwordCount = password.Length;
            if (passwordCount < 8 || passwordCount > 14)
            {
                return false;
            }

            int preCharCode = -1;
            for (int i = 0; i < passwordCount; i++)
            {
                string word = password[i].ToString();
                int charCode = (int)password[i];
                if (!Regex.IsMatch(word, @"[0-9a-zＡ-Ｚ０-９]"))
                {
                    return false;
                }

                if (Math.Abs(charCode - preCharCode) == 1)
                {
                    return false;
                }

                preCharCode = charCode;
            }

            return true;
        }

        /// <summary>
        /// 會員資料更新處理
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <param name="memberData">memberData</param>
        /// <param name="IsStrictPassword">IsStrictPassword</param>
        /// <returns>string</returns>
        private string UpdateMemberDataHandler(MemberInfoDto memberInfo, MemberData memberData, bool IsStrictPassword)
        {
            if (!string.IsNullOrEmpty(memberInfo.BirthDayDate))
                memberData.BirthDayDate = memberInfo.BirthDayDate;

            if (memberInfo.BodyHeight > decimal.Zero)
                memberData.BodyHeight = memberInfo.BodyHeight;

            if (memberInfo.BodyWeight > decimal.Zero)
                memberData.BodyWeight = memberInfo.BodyWeight;

            if (memberInfo.Gender > 0)
                memberData.Gender = memberInfo.Gender;

            if (!string.IsNullOrEmpty(memberInfo.Mobile))
            {
                if (!Utility.IsValidMobile(memberInfo.Mobile))
                {
                    return "手機格式驗證失敗.";
                }

                memberData.Mobile = memberInfo.Mobile;
            }

            if (!string.IsNullOrEmpty(memberInfo.Nickname))
            {
                memberData.Nickname = memberInfo.Nickname;
            }

            if (!string.IsNullOrEmpty(memberInfo.Password))
            {
                if (IsStrictPassword && !this.IsValidPassword(memberInfo.Password))
                {
                    return "密碼格式錯誤.";
                }

                memberData.Password = Utility.EncryptAES(memberInfo.Password);
            }

            if (!string.IsNullOrEmpty(memberInfo.Photo))
            {
                memberData.Photo = memberInfo.Photo;
            }

            return string.Empty;
        }

        #endregion 會員資料

        #region 車輛資料

        /// <summary>
        /// 新增車輛
        /// </summary>
        /// <param name="bikeInfo">bikeInfo</param>
        /// <returns>string</returns>
        public async Task<string> AddBike(BikeInfoDto bikeInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(bikeInfo.MemberID))
                {
                    return "會員編號無效.";
                }

                if (string.IsNullOrEmpty(bikeInfo.BuyDate))
                {
                    return "購買日期無效.";
                }

                if (bikeInfo.ManufactureYaer <= 0)
                {
                    return "車輛出廠年度無效.";
                }

                if (string.IsNullOrEmpty(bikeInfo.Model))
                {
                    return "型號無效.";
                }

                if (string.IsNullOrEmpty(bikeInfo.Photo))
                {
                    return "未上傳車輛照片.";
                }

                if (string.IsNullOrEmpty(bikeInfo.Size))
                {
                    return "尺寸無效.";
                }

                if (string.IsNullOrEmpty(bikeInfo.Type))
                {
                    return "車種無效.";
                }

                bool verifyMemberResult = await this.memberRepository.VerifyMemberList(new string[] { bikeInfo.MemberID });
                if (!verifyMemberResult)
                {
                    return "無會員資料.";
                }

                BikeData bikeData = this.mapper.Map<BikeData>(bikeInfo);
                DateTime createDate = DateTime.Now;
                bikeData.BikeID = $"{Guid.NewGuid().ToString().Substring(0, 6)}-{createDate:yyyy}-{createDate:MMdd}";
                bool isSuccess = await this.bikeRepository.CreateBikeData(bikeData);
                if (!isSuccess)
                {
                    this.logger.LogError($"Add Bike Fail For Create Bike Data >>> Data:{JsonConvert.SerializeObject(bikeData)}");
                    return "新增車輛失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Bike Error >>> Data:{JsonConvert.SerializeObject(bikeInfo)}\n{ex}");
                return "新增車輛發生錯誤.";
            }
        }

        /// <summary>
        /// 取得我的車輛資訊列表
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>string</returns>
        public async Task<Tuple<IEnumerable<BikeInfoDto>, string>> GetMyBikeInfoList(MemberInfoDto memberInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(memberInfo.MemberID))
                {
                    return Tuple.Create<IEnumerable<BikeInfoDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<BikeData> bikeDatas = await this.bikeRepository.GetBikeDataListOfMember(memberInfo.MemberID);
                return Tuple.Create(this.mapper.Map<IEnumerable<BikeInfoDto>>(bikeDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get My Bike Info List Error >>> MemberID:{memberInfo.MemberID}\n{ex}");
                return Tuple.Create<IEnumerable<BikeInfoDto>, string>(null, "取得我的車輛資訊列表發生錯誤.");
            }
        }

        /// <summary>
        /// 移除車輛
        /// </summary>
        /// <param name="bikeInfo">bikeInfo</param>
        /// <returns>string</returns>
        public async Task<string> RemoveBike(BikeInfoDto bikeInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(bikeInfo.BikeID))
                {
                    return "車輛編號無效.";
                }

                bool isSuccess = await this.bikeRepository.DeleteBikeData(bikeInfo.BikeID);
                if (!isSuccess)
                {
                    this.logger.LogError($"Remove Bike Fail For Delete Bike Data >>> BikeID:{bikeInfo.BikeID}");
                    return "移除車輛失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Remove Bike Error >>> BikeID:{bikeInfo.BikeID}\n{ex}");
                return "移除車輛發生錯誤.";
            }
        }

        #endregion 車輛資料
    }
}