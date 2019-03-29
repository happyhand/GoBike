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
        public MemberService(ILogger<MemberService> logger, IMemberRepository memberRepository, IOptions<SmtpSetting> smtpSetting)
        {
            this.logger = logger;
            this.memberRepository = memberRepository;
            this.smtpSetting = smtpSetting;
        }

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>MemberInfoDto</returns>
        public async Task<MemberInfoDto> EditData(string memberID, MemberInfoDto memberInfo)
        {
            try
            {
                MemberData memberData = await this.memberRepository.GetMemebrDataByID(memberID);
                if (memberData == null)
                {
                    return null;
                }

                memberData.BirthDayDate = string.IsNullOrEmpty(memberInfo.BirthDayDate) ? memberData.BirthDayDate : editRequest.BirthDayDate;
                memberData.BodyHeight = string.IsNullOrEmpty(memberInfo.BodyHeight) ? memberData.BodyHeight : Convert.ToDecimal(editRequest.BodyHeight);
                memberData.BodyWeight = string.IsNullOrEmpty(memberInfo.BodyWeight) ? memberData.BodyWeight : Convert.ToDecimal(editRequest.BodyWeight);
                memberData.Gender = string.IsNullOrEmpty(memberInfo.Gender) ? memberData.Gender : editRequest.Gender;
                if (!string.IsNullOrEmpty(editRequest.Mobile))
                {
                    if (!Utility.IsValidMobile(editRequest.Mobile))
                    {
                        return new EditDataRespone() { ResultCode = -2, ResultMessage = "The mobile format error." };
                    }

                    memberData.Mobile = editRequest.Mobile;
                }

                memberData.Nickname = string.IsNullOrEmpty(editRequest.Nickname) ? memberData.Nickname : editRequest.Nickname;
                bool isSuccess = await this.memberRepository.UpdateMemebrData(memberData);
                if (!isSuccess)
                {
                    return new EditDataRespone() { ResultCode = 0, ResultMessage = "Edit Data Fail" };
                }

                return new EditDataRespone() { ResultCode = 1, ResultMessage = "Edit Data Success", MemberData = memberData };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Forget Password Error >>> ID:{editRequest.MemberID}\n{ex}");
                return new EditDataRespone() { ResultCode = -999, ResultMessage = "Edit Data Error" };
            }
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>bool</returns>
        public async Task<bool> ForgetPassword(string email)
        {
            try
            {
                MemberData member = await this.memberRepository.GetMemebrDataByEmail(email);
                if (member == null)
                {
                    return new ForgetPasswordRespone() { ResultCode = -1, ResultMessage = "The email is not exist." };
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
                return new ForgetPasswordRespone() { ResultCode = 1, ResultMessage = "Sent Email" };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Forget Password Error >>> Email:{email}\n{ex}");
                return new ForgetPasswordRespone() { ResultCode = -999, ResultMessage = "Forget Password Error" };
            }
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>MemberInfoDto</returns>
        public async Task<MemberInfoDto> GetMemberInfo(string memberID)
        {
            try
            {
                MemberData memberData = await this.memberRepository.GetMemebrDataByID(memberID);
                if (memberData == null)
                {
                    return new GetMemberInfoRespone() { ResultCode = -1, ResultMessage = "The member is not exist." };
                }

                return new GetMemberInfoRespone() { ResultCode = 1, ResultMessage = "Get Member Info Success", MemberData = memberData };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Info Error >>> ID:{memberID}\n{ex}");
                return new GetMemberInfoRespone() { ResultCode = -999, ResultMessage = "Get Member Info Error" };
            }
        }

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        public async Task<bool> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return new LoginRespone() { ResultCode = -1, ResultMessage = "The email or password is empty.", MemberID = string.Empty, Token = string.Empty };
                }

                MemberData member = await this.memberRepository.GetMemebrDataByEmail(email);
                if (member == null || !Utility.DecryptAES(member.Password).Equals(password))
                {
                    return new LoginRespone() { ResultCode = 0, ResultMessage = "Login Fail", MemberID = string.Empty, Token = string.Empty };
                }

                string token = $"{Utility.EncryptAES(email)}{Utility.SeparateFlag}{Utility.EncryptAES(password)}";

                return new LoginRespone() { ResultCode = 1, ResultMessage = "Login Success", MemberID = member.Id.ToString(), Token = token };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Login Error >>> Email:{email} Password:{password}\n{ex}");
                return new LoginRespone() { ResultCode = -999, ResultMessage = "Login Error", MemberID = string.Empty, Token = string.Empty };
            }
        }

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>bool</returns>
        public async Task<bool> Login(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new LoginRespone() { ResultCode = -2, ResultMessage = "The token is empty.", MemberID = string.Empty, Token = string.Empty };
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
        /// <returns>bool</returns>
        public async Task<bool> Register(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return new RegisterRespone() { ResultCode = -4, ResultMessage = "The email or password is empty." };
                }

                if (!Utility.IsValidEmail(email))
                {
                    return new RegisterRespone() { ResultCode = -3, ResultMessage = "The email format error." };
                }

                if (!this.IsValidPassword(password))
                {
                    return new RegisterRespone() { ResultCode = -2, ResultMessage = "The password format error." };
                }

                bool memberIsExist = await this.memberRepository.GetMemebrDataByEmail(email) != null;
                if (memberIsExist)
                {
                    return new RegisterRespone() { ResultCode = -1, ResultMessage = "The member is existed." };
                }

                MemberData member = new MemberData()
                {
                    Email = email,
                    Password = Utility.EncryptAES(password),
                    CreateDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                };

                bool isSuccess = await this.memberRepository.CreateMember(member);
                if (isSuccess)
                {
                    return new RegisterRespone() { ResultCode = 1, ResultMessage = "Register Member Success" };
                }

                return new RegisterRespone() { ResultCode = 0, ResultMessage = "Register Member Fail" };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Member Error >>> Email:{email} Password:{password}\n{ex}");
                return new RegisterRespone() { ResultCode = -999, ResultMessage = "Register Member Error" };
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

        private bool OnUpdateMemberData(MemberInfoDto memberInfo, out MemberData memberData)
        {
            //if (!string.IsNullOrEmpty(memberInfo.BirthDayDate))
            //    memberData.BirthDayDate = memberInfo.BirthDayDate;

            //if (memberInfo.BodyHeight.HasValue)
            //    memberData.BodyHeight = memberInfo.BodyHeight.Value;

            //if (memberInfo.BodyWeight.HasValue)
            //    memberData.BodyWeight = memberInfo.BodyWeight.Value;

            //if (memberInfo.Gender.HasValue)
            //    memberData.Gender = memberInfo.Gender.Value;

            //if (!string.IsNullOrEmpty(memberInfo.Mobile))
            //    memberData.Mobile = memberInfo.Mobile;

            //if (!string.IsNullOrEmpty(editRequest.Mobile))
            //{
            //    if (!Utility.IsValidMobile(editRequest.Mobile))
            //    {
            //        return new EditDataRespone() { ResultCode = -2, ResultMessage = "The mobile format error." };
            //    }

            //    memberData.Mobile = editRequest.Mobile;
            //}

            //memberData.Nickname = string.IsNullOrEmpty(editRequest.Nickname) ? memberData.Nickname : editRequest.Nickname;
        }
    }
}