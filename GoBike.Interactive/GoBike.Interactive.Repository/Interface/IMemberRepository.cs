using GoBike.Interactive.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.Repository.Interface
{
    /// <summary>
    /// 會員資料庫
    /// </summary>
    public interface IMemberRepository
    {
        /// <summary>
        /// 取得會員資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>MemberData</returns>
        Task<MemberData> GetMemebrData(string memberID);

        /// <summary>
        /// 取得會員資料列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>MemberDatas</returns>
        Task<IEnumerable<MemberData>> GetMemebrDataList(IEnumerable<string> memberIDs);
    }
}