using GoBike.Member.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.Service.Interface
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
        /// <param name="memberInfo">memberInfo</param>
        /// <param name="IsStrictPassword">IsStrictPassword</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        Task<Tuple<MemberInfoDto, string>> EditData(MemberInfoDto memberInfo, bool IsStrictPassword);

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        Task<Tuple<MemberInfoDto, string>> GetMemberInfo(MemberInfoDto memberInfo);

        /// <summary>
        /// 取得會員資訊列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetMemberInfoList(IEnumerable<string> memberIDs);

        /// <summary>
        /// 會員登入
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>Tuple(string, string)</returns>
        Task<Tuple<string, string>> Login(MemberInfoDto memberInfo);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>string</returns>
        Task<string> Register(MemberInfoDto memberInfo);

        /// <summary>
        /// 驗證會員資料
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>string</returns>
        Task<string> VerifyMemberList(IEnumerable<string> memberIDs);

        #endregion 會員資料

        #region 車輛資料

        /// <summary>
        /// 新增車輛
        /// </summary>
        /// <param name="bikeInfo">bikeInfo</param>
        /// <returns>string</returns>
        Task<string> AddBike(BikeInfoDto bikeInfo);

        /// <summary>
        /// 取得我的車輛資訊列表
        /// </summary>
        /// <param name="memberInfo">memberInfo</param>
        /// <returns>string</returns>
        Task<Tuple<IEnumerable<BikeInfoDto>, string>> GetMyBikeInfoList(MemberInfoDto memberInfo);

        /// <summary>
        /// 移除車輛
        /// </summary>
        /// <param name="bikeInfo">bikeInfo</param>
        /// <returns>string</returns>
        Task<string> RemoveBike(BikeInfoDto bikeInfo);

        #endregion 車輛資料
    }
}