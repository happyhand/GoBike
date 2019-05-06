using GoBike.API.Service.Models.Member.Command;
using GoBike.API.Service.Models.Member.Data;
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
        #region 會員資料

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="MemberInfoDto">memberInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> EditData(MemberInfoDto memberInfo);

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="targetData">targetData</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetMemberInfo(string memberID, MemberBaseCommandDto targetData);

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
        /// <param name="files">files</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> UploadPhoto(string memberID, IFormFile file);

        #endregion 會員資料

        #region 會員互動資料

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddBlacklist(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriend(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriendRequest(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteBlacklist(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteFriend(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteRequestForAddFriend(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetAddFriendRequestList(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetBlacklist(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetFriendList(MemberInteractiveCommandDto memberInteractiveCommand);

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="memberInteractiveCommand">memberInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectBeFriend(MemberInteractiveCommandDto memberInteractiveCommand);

        #endregion 會員互動資料
    }
}