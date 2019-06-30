using GoBike.API.Service.Models.Member.Command;
using GoBike.API.Service.Models.Member.Command.Data;
using GoBike.API.Service.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interfacebackup.Member
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
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetMemberInfo(MemberBaseCommandDto memberBaseCommand);

        /// <summary>
        /// 查詢會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="memberSearchCommand">memberSearchCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> InquireMemberInfo(string memberID, MemberSearchCommandDto memberSearchCommand);

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Login(HttpContext httpContext, string email, string password);

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Login(HttpContext httpContext, string token);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Register(MemberBaseCommandDto memberBaseCommand);

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="memberBaseCommand">memberBaseCommand</param>
        /// <returns>HttpResponseMessage</returns>
        Task<ResponseResultDto> ResetPassword(MemberBaseCommandDto memberBaseCommand);

        /// <summary>
        /// 上傳頭像
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="file">file</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> UploadPhoto(string memberID, IFormFile file);
    }
}