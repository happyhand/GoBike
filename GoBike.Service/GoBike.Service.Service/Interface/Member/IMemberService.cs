using GoBike.Service.Repository.Models.Member;
using System;
using System.Threading.Tasks;

namespace GoBike.Service.Service.Interface.Member
{
    public interface IMemberService
    {
        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>string</returns>
        Task<string> EditData(MemberDto memberDto);

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> Login(MemberDto memberDto);

        /// <summary>
        /// 會員登入 (FB)
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> LoginFB(MemberDto memberDto);

        /// <summary>
        /// 會員登入 (Google)
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> LoginGoogle(MemberDto memberDto);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <param name="isVerifyPassword">isVerifyPassword</param>
        /// <returns>string</returns>
        Task<string> Register(MemberDto memberDto, bool isVerifyPassword);

        /// <summary>
        /// 會員重設密碼
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> ResetPassword(MemberDto memberDto);
    }
}