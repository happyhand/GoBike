using GoBike.Member.Service.Models;
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
        /// <param name="memberID">memberID</param>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>MemberInfoDto</returns>
        Task<MemberInfoDto> EditData(string memberID, MemberInfoDto memberInfo);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>bool</returns>
        Task<bool> ForgetPassword(string email);

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>MemberInfoDto</returns>
        Task<MemberInfoDto> GetMemberInfo(string memberID);

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        Task<bool> Login(string email, string password);

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>bool</returns>
        Task<bool> Login(string token);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        Task<bool> Register(string email, string password);
    }
}