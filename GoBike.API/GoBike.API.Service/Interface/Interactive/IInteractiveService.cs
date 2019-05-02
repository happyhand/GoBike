using GoBike.API.Service.Models.Command;
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddBlacklist(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddFriendRequest(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteBlacklist(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteRequestForAddFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetAddFriendRequestList(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetBlacklist(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetFriendList(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectBeFriend(InteractiveCommandDto interactiveCommand);

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchFriend(InteractiveCommandDto interactiveCommand);
    }
}