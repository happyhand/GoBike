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
    }
}