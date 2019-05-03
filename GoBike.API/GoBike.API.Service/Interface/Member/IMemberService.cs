using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Http;
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
        /// <param name="MemberInfoDto">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> EditData(MemberInfoDto memberInfo);

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberBase">memberBase</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetMemberInfo(MemberBaseDto memberBase);

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
        /// <param name="memberBase">memberBase</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Register(MemberBaseDto memberBase);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="memberBase">memberBase</param>
        /// <returns>HttpResponseMessage</returns>
        Task<ResponseResultDto> ResetPassword(MemberBaseDto memberBase);

        /// <summary>
        /// 搜尋會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="targetMemberBase">targetMemberBase</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchMemberInfo(string memberID, MemberBaseDto targetMemberBase);

        /// <summary>
        /// 上傳頭像
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="files">files</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> UploadPhoto(string memberID, IFormFile file);
    }
}