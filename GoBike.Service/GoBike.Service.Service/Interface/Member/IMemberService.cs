using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoBike.Service.Service.Models.Member;

namespace GoBike.Service.Service.Interface.Member
{
    public interface IMemberService
    {
        #region 註冊\登入

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
        /// <param name="reTryCount">reTryCount</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> LoginFB(MemberDto memberDto, int reTryCount);

        /// <summary>
        /// 會員登入 (Google)
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <param name="reTryCount">reTryCount</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> LoginGoogle(MemberDto memberDto, int reTryCount);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <param name="isVerifyPassword">isVerifyPassword</param>
        /// <returns>string</returns>
        Task<string> Register(MemberDto memberDto, bool isVerifyPassword);

        #endregion 註冊\登入

        #region 會員資料

        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>string</returns>
        Task<string> EditData(MemberDto memberDto);

        /// <summary>
        /// 會員重設密碼
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> ResetPassword(MemberDto memberDto);

        /// <summary>
        /// 搜尋會員
        /// </summary>
        /// <param name="searchMemberDto">searchMemberDto</param>
        /// <returns>Tuple(MemberDto, string)</returns>
        Task<Tuple<MemberDto, string>> SearchMember(MemberDto searchMemberDto);

        /// <summary>
        /// 搜尋會員列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(MemberDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberDto>, string>> SearchMemberList(IEnumerable<string> memberIDs);

        #endregion 會員資料

        #region 騎乘資料

        /// <summary>
        /// 新增騎乘資料
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>string</returns>
        Task<string> AddRideData(RideDto rideDto);

        /// <summary>
        /// 取得騎乘資料
        /// </summary>
        /// <param name="rideDto">rideDto</param>
        /// <returns>Tuple(RideDto, string)</returns>
        Task<Tuple<RideDto, string>> GetRideData(RideDto rideDto);

        /// <summary>
        /// 取得會員的騎乘資料列表
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <returns>Tuple(RideDtos, string)</returns>
        Task<Tuple<IEnumerable<RideDto>, string>> GetRideDataListOfMember(MemberDto memberDto);

        #endregion 騎乘資料

        #region 互動資料

        /// <summary>
        /// 取得被加入好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>Tuple(MemberDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberDto>, string>> GetBeAddFriendList(string memberID);

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>Tuple(MemberDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberDto>, string>> GetBlackList(string memberID);

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>Tuple(MemberDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberDto>, string>> GetFriendList(string memberID);

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>string</returns>
        Task<string> JoinBlack(InteractiveDto interactiveDto);

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>string</returns>
        Task<string> JoinFriend(InteractiveDto interactiveDto);

        /// <summary>
        /// 移除黑名單
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>string</returns>
        Task<string> RemoveBlack(InteractiveDto interactiveDto);

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>string</returns>
        Task<string> RemoveFriend(InteractiveDto interactiveDto);

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveDto">interactiveDto</param>
        /// <returns>Tuple(MemberDto, string)</returns>
        Task<Tuple<MemberDto, string>> SearchFriend(InteractiveDto interactiveDto);

        #endregion 互動資料
    }
}