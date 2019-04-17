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
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetMemberInfo(MemberInfoDto memberInfo);

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Login(MemberInfoDto memberInfo);

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Login(string token);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Register(MemberInfoDto memberInfo);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> ResetPassword(MemberInfoDto memberInfo);

        /// <summary>
        /// 上傳大頭貼
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="files">files</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> UploadPhoto(string memberID, IFormFile file);
    }
}