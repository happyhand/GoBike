using AutoMapper;
using GoBike.Member.Core.Models;
using GoBike.Member.Core.Resource;
using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
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
        /// smtpSetting
        /// </summary>
        private readonly IOptions<SmtpSetting> smtpSetting;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="memberRepository">memberRepository</param>
        public MemberService(ILogger<MemberService> logger, IMapper mapper, IMemberRepository memberRepository, IOptions<SmtpSetting> smtpSetting)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberRepository = memberRepository;
            this.smtpSetting = smtpSetting;
        }

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        public async Task<Tuple<MemberInfoDto, string>> EditData(MemberInfoDto memberInfo)
        {
            try
            {
                MemberData memberData = await this.memberRepository.GetMemebrDataByID(memberInfo.MemberID);
                if (memberData == null)
                {
                    this.logger.LogError($"Edit Data Error >>> ID:{memberInfo.MemberID}\nThe member is not exist.");
                    return Tuple.Create<MemberInfoDto, string>(null, "會員不存在.");
                }

                string updateMemberDataHandlerResult = this.UpdateMemberDataHandler(memberInfo, ref memberData);
                if (!string.IsNullOrEmpty(updateMemberDataHandlerResult))
                {
                    return Tuple.Create<MemberInfoDto, string>(null, updateMemberDataHandlerResult);
                }

                bool isSuccess = await this.memberRepository.UpdateMemebrData(memberData);
                if (!isSuccess)
                {
                    return Tuple.Create<MemberInfoDto, string>(null, "會員資料更新失敗.");
                }

                return Tuple.Create(this.mapper.Map<MemberInfoDto>(memberData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> ID:{memberInfo.MemberID} Data:{memberInfo.GetData}\n{ex}");
                return Tuple.Create<MemberInfoDto, string>(null, "會員資料更新發生錯誤.");
            }
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>string</returns>
        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                MemberData member = await this.memberRepository.GetMemebrDataByEmail(email);
                if (member == null)
                {
                    return "會員不存在.";
                }

                MailContext mailContext = new MailContext()
                {
                    SmtpServer = smtpSetting.Value.SmtpServer,
                    SmtpMail = smtpSetting.Value.SmtpMail,
                    SmtpPassword = smtpSetting.Value.SmtpPassword,
                    ToEmail = email,
                    ToUserName = member.Nickname,
                    FromEmail = smtpSetting.Value.SmtpMail,
                    FromUserName = "GoBike",
                    Subject = "【忘記密碼】系統通知信",
                    Body = $"<p>親愛的 {member.Nickname} 用戶您好</p><p>您於 <span style='font-weight:bold; color:blue;'>{DateTime.Now:yyyy/MM/dd HH:mm:ss}</span> 查詢密碼</p><p>此帳號密碼為：<span style='font-weight:bold; color:blue;'>{Utility.DecryptAES(member.Password)}</span></p><br><br><br><p>※本電子郵件係由系統自動發送，請勿直接回覆本郵件。</p>"
                };

                Utility.SendMail(mailContext);
                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Forget Password Error >>> Email:{email}\n{ex}");
                return "發送郵件發生錯誤.";
            }
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        public async Task<Tuple<MemberInfoDto, string>> GetMemberInfo(string memberID)
        {
            try
            {
                MemberData memberData = await this.memberRepository.GetMemebrDataByID(memberID);
                if (memberData == null)
                {
                    return Tuple.Create<MemberInfoDto, string>(null, "會員不存在.");
                }

                return Tuple.Create(this.mapper.Map<MemberInfoDto>(memberData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info Error >>> ID:{memberID}\n{ex}");
                return Tuple.Create<MemberInfoDto, string>(null, "取得會員資料發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>Tuple(LoginInfoDto, string)</returns>
        public async Task<Tuple<LoginInfoDto, string>> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return Tuple.Create<LoginInfoDto, string>(null, "信箱或密碼無效.");
                }

                MemberData member = await this.memberRepository.GetMemebrDataByEmail(email);
                if (member == null)
                {
                    return Tuple.Create<LoginInfoDto, string>(null, "無法根據信箱查詢到相關會員.");
                }

                if (!Utility.DecryptAES(member.Password).Equals(password))
                {
                    return Tuple.Create<LoginInfoDto, string>(null, "密碼驗證失敗.");
                }

                string token = $"{Utility.EncryptAES(email)}{Utility.SeparateFlag}{Utility.EncryptAES(password)}";
                return Tuple.Create(new LoginInfoDto() { MemberID = member.MemberID, Token = token }, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Error >>> Email:{email} Password:{password}\n{ex}");
                return Tuple.Create<LoginInfoDto, string>(null, "登入發生錯誤.");
            }
        }

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>Tuple(LoginInfoDto, string)</returns>
        public async Task<Tuple<LoginInfoDto, string>> Login(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Tuple.Create<LoginInfoDto, string>(null, "自動登入驗證碼無效.");
            }

            string[] dataArr = token.Split(Utility.SeparateFlag);
            string account = Utility.DecryptAES(dataArr[0]);
            string password = Utility.DecryptAES(dataArr[1]);
            return await this.Login(account, password);
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>string</returns>
        public async Task<string> Register(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return "信箱或密碼無效.";
                }

                if (!Utility.IsValidEmail(email))
                {
                    return "信箱格式錯誤.";
                }

                if (!this.IsValidPassword(password))
                {
                    return "密碼格式錯誤.";
                }

                bool emailIsRegister = await this.memberRepository.GetMemebrDataByEmail(email) != null;
                if (emailIsRegister)
                {
                    return "此信箱已經被註冊.";
                }

                MemberData member = this.CreateNewMemberData(email, password);
                bool isSuccess = await this.memberRepository.CreateMember(member);
                if (!isSuccess)
                {
                    return "會員註冊失敗";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Member Error >>> Email:{email} Password:{password}\n{ex}");
                return "會員註冊發生錯誤";
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
        /// <returns>string</returns>
        private string UpdateMemberDataHandler(MemberInfoDto memberInfo, ref MemberData memberData)
        {
            if (!string.IsNullOrEmpty(memberInfo.BirthDayDate))
                memberData.BirthDayDate = memberInfo.BirthDayDate;

            if (memberInfo.BodyHeight.HasValue)
                memberData.BodyHeight = memberInfo.BodyHeight.Value;

            if (memberInfo.BodyWeight.HasValue)
                memberData.BodyWeight = memberInfo.BodyWeight.Value;

            if (memberInfo.Gender.HasValue)
                memberData.Gender = memberInfo.Gender.Value;

            if (!string.IsNullOrEmpty(memberInfo.Mobile))
            {
                if (!Utility.IsValidMobile(memberInfo.Mobile))
                {
                    this.logger.LogError($"Update Member Data Handler Error >>> id:{memberData.Id} Data:{memberInfo.GetData}\nMobile valid fail.");
                    return "手機格式驗證失敗.";
                }

                memberData.Mobile = memberInfo.Mobile;
            }

            if (!string.IsNullOrEmpty(memberInfo.Nickname))
            {
                memberData.Nickname = memberInfo.Nickname;
            }

            return string.Empty;
        }

        /// <summary>
        /// 創建新會員資料
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>MemberData</returns>
        private MemberData CreateNewMemberData(string email, string password)
        {
            DateTime createDate = DateTime.Now;
            string memberID = $"{Guid.NewGuid().ToString().Substring(0, 4)}-{createDate:yyyy}-{createDate:MMdd}";
            return new MemberData()
            {
                MemberID = memberID,
                Email = email,
                Password = Utility.EncryptAES(password),
                CreateDate = createDate.ToString("yyyy/MM/dd HH:mm:ss"),
            };
        }
    }
}