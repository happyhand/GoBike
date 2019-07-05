using AutoMapper;
using GoBike.Service.Core.Resource;
using GoBike.Service.Repository.Interface.Member;
using GoBike.Service.Repository.Models.Member;
using GoBike.Service.Service.Interface.Member;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoBike.Service.Service.Managers.Member
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
        /// memberRepository
        /// </summary>
        private readonly IMemberRepository memberRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="memberRepository">memberRepository</param>
        public MemberService(ILogger<MemberService> logger, IMapper mapper, IMemberRepository memberRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberRepository = memberRepository;
        }

        #region 註冊\登入

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        public async Task<Tuple<string, string>> Login(MemberDto memberDto)
        {
            try
            {
                if (string.IsNullOrEmpty(memberDto.Email) || string.IsNullOrEmpty(memberDto.Password))
                {
                    return Tuple.Create(string.Empty, "信箱或密碼無效.");
                }

                MemberData memberData = await this.memberRepository.GetMemberDataByEmail(memberDto.Email);
                if (memberData == null)
                {
                    return Tuple.Create(string.Empty, "無法根據信箱查詢到相關會員.");
                }

                if (string.IsNullOrEmpty(memberData.Password))
                {
                    return Tuple.Create(string.Empty, "密碼驗證失敗.");
                }

                string dbPassword = Utility.DecryptAES(memberData.Password);
                if (!dbPassword.Equals(memberDto.Password))
                {
                    return Tuple.Create(string.Empty, "密碼驗證失敗.");
                }

                bool updateMemberLoginDateResult = await this.memberRepository.UpdateMemberLoginDate(memberData.MemberID, DateTime.Now);
                if (!updateMemberLoginDateResult)
                {
                    return Tuple.Create(string.Empty, "會員登入失敗.");
                }

                return Tuple.Create(memberData.MemberID, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Error >>> Email:{memberDto.Email} Password:{memberDto.Password}\n{ex}");
                return Tuple.Create(string.Empty, "會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入 (FB)
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        public async Task<Tuple<string, string>> LoginFB(MemberDto memberDto)
        {
            try
            {
                if (string.IsNullOrEmpty(memberDto.FBToken))
                {
                    return Tuple.Create(string.Empty, "無效的登入驗證碼.");
                }

                MemberData memberData = await this.memberRepository.GetMemberDataByFBToken(memberDto.FBToken);
                if (memberData != null)
                {
                    bool updateMemberLoginDateResult = await this.memberRepository.UpdateMemberLoginDate(memberData.MemberID, DateTime.Now);
                    if (!updateMemberLoginDateResult)
                    {
                        return Tuple.Create(string.Empty, "會員登入失敗.");
                    }

                    return Tuple.Create(memberData.MemberID, string.Empty);
                }

                string registerResult = await this.Register(memberDto, false);
                if (!string.IsNullOrEmpty(registerResult))
                {
                    return Tuple.Create(string.Empty, registerResult);
                }

                return await this.LoginFB(memberDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login FB Error >>> FBToken:{memberDto.FBToken}\n{ex}");
                return Tuple.Create(string.Empty, "會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入 (Google)
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        public async Task<Tuple<string, string>> LoginGoogle(MemberDto memberDto)
        {
            try
            {
                if (string.IsNullOrEmpty(memberDto.GoogleToken))
                {
                    return Tuple.Create(string.Empty, "無效的登入驗證碼.");
                }

                MemberData memberData = await this.memberRepository.GetMemberDataByGoogleToken(memberDto.GoogleToken);
                if (memberData != null)
                {
                    bool updateMemberLoginDateResult = await this.memberRepository.UpdateMemberLoginDate(memberData.MemberID, DateTime.Now);
                    if (!updateMemberLoginDateResult)
                    {
                        return Tuple.Create(string.Empty, "會員登入失敗.");
                    }

                    return Tuple.Create(memberData.MemberID, string.Empty);
                }

                string registerResult = await this.Register(memberDto, false);
                if (!string.IsNullOrEmpty(registerResult))
                {
                    return Tuple.Create(string.Empty, registerResult);
                }

                return await this.LoginGoogle(memberDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Google Error >>> GoogleToken:{memberDto.GoogleToken}\n{ex}");
                return Tuple.Create(string.Empty, "會員登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <param name="isVerifyPassword">isVerifyPassword</param>
        /// <returns>string</returns>
        public async Task<string> Register(MemberDto memberDto, bool isVerifyPassword)
        {
            try
            {
                string verifyMemberRegisterResult = await this.VerifyMemberRegister(memberDto, isVerifyPassword);
                if (!string.IsNullOrEmpty(verifyMemberRegisterResult))
                {
                    return verifyMemberRegisterResult;
                }

                MemberData memberData = this.CreateMemberData(memberDto);
                bool isSuccess = await this.memberRepository.CreateMemberData(memberData);
                if (!isSuccess)
                {
                    return "會員註冊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Data:{JsonConvert.SerializeObject(memberDto)}\n{ex}");
                return "會員註冊發生錯誤.";
            }
        }

        /// <summary>
        /// 建立會員資料
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>MemberData</returns>
        private MemberData CreateMemberData(MemberDto memberDto)
        {
            DateTime createDate = DateTime.Now;
            MemberData memberData = new MemberData()
            {
                CreateDate = createDate,
                Email = memberDto.Email,
                Password = string.IsNullOrEmpty(memberDto.Password) ? string.Empty : Utility.EncryptAES(memberDto.Password),
                LoginDate = createDate,
                FBToken = memberDto.FBToken,
                GoogleToken = memberDto.GoogleToken
            };

            return memberData;
        }

        /// <summary>
        /// Email 格式驗證
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>bool</returns>
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// 驗證密碼格式
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        private bool IsValidPassword(string password)
        {
            //// 待確認驗證方式
            //int passwordCount = password.Length;
            //if (passwordCount < 8 || passwordCount > 14)
            //{
            //    return false;
            //}

            //int preCharCode = -1;
            //for (int i = 0; i < passwordCount; i++)
            //{
            //    string word = password[i].ToString();
            //    int charCode = (int)password[i];
            //    if (!Regex.IsMatch(word, @"[0-9a-zＡ-Ｚ０-９]"))
            //    {
            //        return false;
            //    }

            //    if (Math.Abs(charCode - preCharCode) == 1)
            //    {
            //        return false;
            //    }

            //    preCharCode = charCode;
            //}

            return true;
        }

        /// <summary>
        /// 驗證會員註冊資料
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <param name="isVerifyPassword">isVerifyPassword</param>
        /// <returns>string</returns>
        private async Task<string> VerifyMemberRegister(MemberDto memberDto, bool isVerifyPassword)
        {
            if (string.IsNullOrEmpty(memberDto.Email))
            {
                return "信箱無效.";
            }

            if (!this.IsValidEmail(memberDto.Email))
            {
                return "信箱格式錯誤.";
            }

            if (isVerifyPassword)
            {
                if (string.IsNullOrEmpty(memberDto.Password))
                {
                    return "密碼無效.";
                }

                if (!this.IsValidPassword(memberDto.Password))
                {
                    return "密碼格式錯誤.";
                }
            }

            bool emailHasRegister = await this.memberRepository.GetMemberDataByEmail(memberDto.Email) != null;
            if (emailHasRegister)
            {
                return "此信箱已經被註冊.";
            }

            return string.Empty;
        }

        #endregion 註冊\登入

        #region 會員資料

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>string</returns>
        public async Task<string> EditData(MemberDto memberDto)
        {
            try
            {
                MemberData memberData = await this.memberRepository.GetMemberDataByMemberID(memberDto.MemberID);
                if (memberData == null)
                {
                    return "無會員資料.";
                }

                string updateMemberDataHandlerResult = this.UpdateMemberDataHandler(memberDto, memberData, true);
                if (!string.IsNullOrEmpty(updateMemberDataHandlerResult))
                {
                    return updateMemberDataHandlerResult;
                }

                bool updateMemebrDataResult = await this.memberRepository.UpdateMemberData(memberData);
                if (!updateMemebrDataResult)
                {
                    return "會員資料更新失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(memberDto)}\n{ex}");
                return "會員編輯發生錯誤.";
            }
        }

        /// <summary>
        /// 會員重設密碼
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        public async Task<Tuple<string, string>> ResetPassword(MemberDto memberDto)
        {
            try
            {
                if (string.IsNullOrEmpty(memberDto.Email))
                {
                    return Tuple.Create(string.Empty, "信箱無效.");
                }

                MemberData memberData = await this.memberRepository.GetMemberDataByEmail(memberDto.Email);
                if (memberData == null)
                {
                    return Tuple.Create(string.Empty, "無會員資料.");
                }

                string password = Guid.NewGuid().ToString().Substring(0, 8);
                memberDto.Password = password;
                string updateMemberDataHandlerResult = this.UpdateMemberDataHandler(memberDto, memberData, false);
                if (!string.IsNullOrEmpty(updateMemberDataHandlerResult))
                {
                    return Tuple.Create(string.Empty, updateMemberDataHandlerResult);
                }

                bool updateMemebrDataResult = await this.memberRepository.UpdateMemberData(memberData);
                if (!updateMemebrDataResult)
                {
                    return Tuple.Create(string.Empty, "會員資料更新失敗.");
                }

                return Tuple.Create(password, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reset Password Error >>> Email:{memberDto.Email}\n{ex}");
                return Tuple.Create(string.Empty, "會員重設密碼發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋會員
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(MemberDto, string)</returns>
        public async Task<Tuple<MemberDto, string>> SearchMember(MemberDto memberDto)
        {
            try
            {
                MemberData memberData = null;
                if (!string.IsNullOrEmpty(memberDto.MemberID))
                {
                    memberData = await this.memberRepository.GetMemberDataByMemberID(memberDto.MemberID);
                }
                else if (!string.IsNullOrEmpty(memberDto.Email))
                {
                    memberData = await this.memberRepository.GetMemberDataByEmail(memberDto.Email);
                }
                else
                {
                    return Tuple.Create<MemberDto, string>(null, "無效的查詢參數.");
                }

                if (memberData == null)
                {
                    return Tuple.Create<MemberDto, string>(null, "無會員資料.");
                }

                return Tuple.Create(this.mapper.Map<MemberDto>(memberData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Member Error >>> MemberID:{memberDto.MemberID} Email:{memberDto.Email}\n{ex}");
                return Tuple.Create<MemberDto, string>(null, "搜尋會員發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋會員列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(MemberDtos, string)</returns>
        public async Task<Tuple<IEnumerable<MemberDto>, string>> SearchMemberList(IEnumerable<string> memberIDs)
        {
            try
            {
                if (memberIDs == null || !memberIDs.Any())
                {
                    this.logger.LogError($"Search Member List Error For Not Member IDs");

                    return Tuple.Create<IEnumerable<MemberDto>, string>(new MemberDto[] { }, string.Empty);
                }

                IEnumerable<MemberData> memberDatas = await this.memberRepository.GetMemberDataList(memberIDs);
                return Tuple.Create(this.mapper.Map<IEnumerable<MemberDto>>(memberDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Member List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return Tuple.Create<IEnumerable<MemberDto>, string>(null, "搜尋會員列表發生錯誤.");
            }
        }

        /// <summary>
        /// 會員資料更新處理
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <param name="memberData">memberData</param>
        /// <param name="isStrictPassword">isStrictPassword</param>
        /// <returns>string</returns>
        private string UpdateMemberDataHandler(MemberDto memberDto, MemberData memberData, bool isStrictPassword)
        {
            if (!string.IsNullOrEmpty(memberDto.Password))
            {
                //// 第三方串接無法修改密碼 (TODO 待討論)
                if (string.IsNullOrEmpty(memberData.Password))
                {
                    this.logger.LogError($"Update Member Data Handler Error For Edit Other Platform Password.");
                    return "修改密碼發生錯誤.";
                }

                if (isStrictPassword && !this.IsValidPassword(memberDto.Password))
                {
                    return "密碼格式錯誤.";
                }

                memberData.Password = Utility.EncryptAES(memberDto.Password);
            }

            return string.Empty;
        }

        #endregion 會員資料
    }
}