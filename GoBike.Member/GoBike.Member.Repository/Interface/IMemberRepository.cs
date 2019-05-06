using GoBike.Member.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.Repository.Interface
{
    /// <summary>
    /// 會員資料庫
    /// </summary>
    public interface IMemberRepository
    {
        /// <summary>
        /// 建立會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>bool</returns>
        Task<bool> CreateMemberData(MemberData memberData);

        /// <summary>
        /// 刪除會員資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteMemebrData(string memberID);

        /// <summary>
        /// 取得會員序號
        /// </summary>
        /// <returns>Tuple(long, string)</returns>
        Task<Tuple<long, string>> GetMemberSerialNumber();

        /// <summary>
        /// 取得會員資料 (By Email)
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>MemberData</returns>
        Task<MemberData> GetMemebrDataByEmail(string email);

        /// <summary>
        /// 取得會員資料 (By MemberID)
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>MemberData</returns>
        Task<MemberData> GetMemebrDataByMemberID(string memberID);

        /// <summary>
        /// 取得會員資料 (By Mobile)
        /// </summary>
        /// <param name="mobile">mobile</param>
        /// <returns>MemberData</returns>
        Task<MemberData> GetMemebrDataByMobile(string mobile);

        /// <summary>
        /// 取得會員資料列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>MemberDatas</returns>
        Task<IEnumerable<MemberData>> GetMemebrDataList(IEnumerable<string> memberIDs);

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateMemebrData(MemberData memberData);

        /// <summary>
        /// 更新會員登入日期資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="loginDate">loginDate</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateMemebrLoginDate(string memberID, DateTime loginDate);

        /// <summary>
        /// 驗證會員資料
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>bool</returns>
        Task<bool> VerifyMemberList(IEnumerable<string> memberIDs);
    }
}