using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Member
{
    /// <summary>
    /// 會員服務
    /// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// 會員編輯
        /// </summary>
        /// <param name="editRequest">editRequest</param>
        /// <returns>EditRespone</returns>
        Task<EditDataRespone> EditData(EditDataRequest editRequest);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="email">email</param>
        Task<ForgetPasswordRespone> ForgetPassword(string email);

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>GetMemberInfoRespone</returns>
        Task<GetMemberInfoRespone> GetMemberInfo(string memberID);

        /// <summary>
        /// 會員登入 (normal)
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>Tuple</returns>
        Task<LoginRespone> Login(string email, string password);

        /// <summary>
        /// 會員登入 (token)
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>Tuple</returns>
        Task<LoginRespone> Login(string token);

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>Tuple</returns>
        Task<RegisterRespone> Register(string email, string password);
    }
}