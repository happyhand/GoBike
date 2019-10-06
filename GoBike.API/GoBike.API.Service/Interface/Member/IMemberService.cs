using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Member
{
    /// <summary>
    /// 會員服務
    /// </summary>
    public interface IMemberService
    {
        #region 註冊\登入

        /// <summary>
        /// 刪除會員 Session ID
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="sessionID">sessionID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteSessionID(string memberID, string sessionID);

        /// <summary>
        /// 延長會員 Session ID 期限
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="sessionID">sessionID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> ExtendSessionIDExpire(string memberID, string sessionID);

        /// <summary>
        /// 會員登入
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
        /// 會員登入 (FB)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> LoginFB(string email, string token);

        /// <summary>
        /// 會員登入 (Google)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="token">token</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> LoginGoogle(string email, string token);

        /// <summary>
        /// 紀錄會員 Session ID
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="sessionID">sessionID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RecordSessionID(string memberID, string sessionID);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Register(string email, string password);

        #endregion 註冊\登入

        #region 會員資料

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> EditData(MemberDto memberDto);

        /// <summary>
        /// 取得會員設定資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetSettingData(string memberID);

        /// <summary>
        /// 會員重設密碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> ResetPassword(string email);

        /// <summary>
        /// 搜尋會員
        /// </summary>
        /// <param name="searchKey">searchKey</param>
        /// <param name="searcher">searcher</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchMember(string searchKey, string searcher);

        #endregion 會員資料

        #region 騎乘資料

        /// <summary>
        /// 新增騎乘資料
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AddRideData(RideDto rideDto);

        #endregion 騎乘資料

        #region 互動資料

        /// <summary>
        /// 取得被加入好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetBeAddFriendList(string memberID);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetBlackList(string memberID);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetFriendList(string memberID);

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> JoinBlack(InteractiveDto interactiveDto);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> JoinFriend(InteractiveDto interactiveDto);

        /// <summary>
        /// 移除黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RemoveBlack(InteractiveDto interactiveDto);

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RemoveFriend(InteractiveDto interactiveDto);

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchFriend(InteractiveDto interactiveDto);

        #endregion 互動資料
    }
}