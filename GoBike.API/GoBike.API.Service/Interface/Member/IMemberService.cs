using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Member
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
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> EditData(MemberInfoDto memberInfo);

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetMemberInfo(string memberID);

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Login(string email, string password);

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Login(string token);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Register(string email, string password);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> ResetPassword(string email);
    }
}