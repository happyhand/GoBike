using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Verifier
{
    /// <summary>
    /// 驗證碼服務
    /// </summary>
    public interface IVerifierService
    {
        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="email">email</param>
        /// <returns>string</returns>
        Task<string> GetVerifierCode(string type, string email);

        /// <summary>
        /// 驗證碼是否有效
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="email">email</param>
        /// <param name="VerifierCode">VerifierCode</param>
        /// <returns>bool</returns>
        Task<bool> IsValidVerifierCode(string type, string email, string VerifierCode);

        /// <summary>
        /// 發送驗證碼
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="email">email</param>
        /// <param name="VerifierCode">VerifierCode</param>
        /// <param name="emailContext">emailContext</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SendVerifierCode(string type, string email, string VerifierCode, EmailContext emailContext);
    }
}