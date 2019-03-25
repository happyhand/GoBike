using GoBike.API.Repository.Models;
using System.Threading.Tasks;

namespace GoBike.API.Repository.Interface
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
        Task<bool> CreateMember(MemberData memberData);

        /// <summary>
        /// 刪除會員資料
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>bool</returns>
        Task<bool> DeleteMemebrData(string id);

        /// <summary>
        /// 取得會員資料 (By Email)
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>MemberData</returns>
        Task<MemberData> GetMemebrDataByEmail(string email);

        /// <summary>
        /// 取得會員資料 (By Id)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>MemberData</returns>
        Task<MemberData> GetMemebrDataByID(string id);

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateMemebrData(MemberData memberData);
    }
}