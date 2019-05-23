using GoBike.API.Service.Models.Email;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Verifier.Command;
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
        /// <param name="verifierCommand">verifierCommand</param>
        /// <returns>string</returns>
        Task<string> GetVerifierCode(VerifierCommandDto verifierCommand);

        /// <summary>
        /// 驗證碼是否有效
        /// </summary>
        /// <param name="verifierCommand">verifierCommand</param>
        /// <returns>bool</returns>
        Task<bool> IsValidVerifierCode(VerifierCommandDto verifierCommand);

        /// <summary>
        /// 發送驗證碼
        /// </summary>
        /// <param name="verifierCommand">verifierCommand</param>
        /// <param name="emailContext">emailContext</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SendVerifierCode(VerifierCommandDto verifierCommand, EmailContext emailContext);
    }
}