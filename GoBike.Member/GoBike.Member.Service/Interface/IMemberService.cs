using GoBike.Member.Service.Models;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.Service.Interface
{
    /// <summary>
    /// 會員服務
    /// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        Task<Tuple<MemberInfoDto, string>> EditData(MemberInfoDto memberInfo);

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        Task<Tuple<MemberInfoDto, string>> GetMemberInfo(MemberInfoDto memberInfo);

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> Login(string email, string password);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>string</returns>
        Task<string> Register(string email, string password);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> ResetPassword(string email);
    }
}