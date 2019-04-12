using GoBike.Member.Repository.Models;
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
        Task<MemberData> GetMemebrDataByID(string memberID);

        /// <summary>
        /// 取得會員資料 (By Mobile)
        /// </summary>
        /// <param name="mobile">mobile</param>
        /// <returns>MemberData</returns>
        Task<MemberData> GetMemebrDataByMobile(string mobile);

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateMemebrData(MemberData memberData);
    }
}