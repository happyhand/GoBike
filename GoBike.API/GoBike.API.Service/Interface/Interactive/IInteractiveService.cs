using GoBike.API.Service.Interactive;
using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Interactive
{
    /// <summary>
    /// 好友服務
    /// </summary>
    public interface IInteractiveService
    {
        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddBlacklist(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriendRequest(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteBlacklist(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteRequestForAddFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetAddFriendRequestList(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetBlacklist(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetFriendList(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectBeFriend(InteractiveInfoDto interactiveInfo);

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchFriend(InteractiveInfoDto interactiveInfo);
    }
}