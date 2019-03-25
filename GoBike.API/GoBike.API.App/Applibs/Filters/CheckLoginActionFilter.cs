using GoBike.API.App.Models.Response;
using GoBike.API.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoBike.API.App.Applibs.Filters
{
    /// <summary>
    /// 登入狀態檢測
    /// </summary>
    public class CheckLoginActionFilter : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// loginFlag
        /// </summary>
        private readonly bool loginFlag;

        /// <summary>
        /// 建構式
        /// </summary>
        public CheckLoginActionFilter()
        {
            this.loginFlag = false;
        }

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="loginFlag">loginFlag</param>
        public CheckLoginActionFilter(bool loginFlag)
        {
            this.loginFlag = loginFlag;
        }

        /// <summary>
        /// 檢測篩檢
        /// </summary>
        /// <param name="context">context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string memberID = context.HttpContext.Session.GetObject<string>(Utility.Session_MemberID);
            bool isSuccess = true;
            if (this.loginFlag && string.IsNullOrEmpty(memberID))
            {
                isSuccess = false;
            }
            else if (!this.loginFlag && !string.IsNullOrEmpty(memberID))
            {
                isSuccess = false;
            }

            if (!isSuccess)
            {
                ResultModel resultModel = new ResultModel()
                {
                    ResultCode = -666,
                    ResultMessage = "Login status error"
                };
                context.Result = new JsonResult(resultModel);
            }
        }
    }
}