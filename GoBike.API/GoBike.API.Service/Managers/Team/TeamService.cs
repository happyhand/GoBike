using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Command;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Managers.Team
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public class TeamService : ITeamService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<TeamService> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public TeamService(ILogger<TeamService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 取得申請請求列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetApplyForRequestList(TeamCommandDto teamCommand)
        {
            try
            {
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyTeamCommandResult
                    };
                }

                string postData = JsonConvert.SerializeObject(teamCommand);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/team/GetApplyForRequestList", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
                    postData = JsonConvert.SerializeObject(memberIDs);
                    httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo/list", postData);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        return new ResponseResultDto()
                        {
                            Ok = true,
                            Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberInfoDto>>()
                        };
                    }
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Apply For Request List Error >>> TemaID:{teamCommand.TeamID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得申請請求列表發生錯誤."
                };
            }
        }

        /// <summary>
        /// 驗證車隊指令資料
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isVerifyExaminer">isVerifyExaminer</param>
        /// <param name="isVerifyTarger">isVerifyTarger</param>
        /// <param name="isVerifyData">isVerifyData</param>
        /// <returns>string</returns>
        private string VerifyTeamCommand(TeamCommandDto teamCommand, bool isVerifyExaminer, bool isVerifyTarger, bool isVerifyData)
        {
            if (teamCommand == null)
            {
                return "車隊指令資料不存在.";
            }

            if (string.IsNullOrEmpty(teamCommand.TeamID))
            {
                return "車隊編號無效.";
            }

            if (isVerifyExaminer)
            {
                if (string.IsNullOrEmpty(teamCommand.ExaminerID))
                {
                    return "審查者會員編號無效.";
                }
            }

            if (isVerifyTarger)
            {
                if (string.IsNullOrEmpty(teamCommand.TargetID))
                {
                    return "目標者會員編號無效.";
                }
            }

            if (isVerifyData)
            {
                if (teamCommand.Data == null)
                {
                    return "無車隊資訊.";
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證車隊搜尋指令資料
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <param name="isVerifyTarger">isVerifyTarger</param>
        /// <returns>string</returns>
        private string VerifyTeamSearchCommand(TeamSearchCommandDto teamSearchCommand)
        {
            if (teamSearchCommand == null)
            {
                return "車隊搜尋指令資料不存在.";
            }

            if (string.IsNullOrEmpty(teamSearchCommand.SearcherID))
            {
                return "搜尋者會員編號無效.";
            }

            if (string.IsNullOrEmpty(teamSearchCommand.SearchKey))
            {
                return "無車隊搜尋關鍵字.";
            }

            return string.Empty;
        }
    }
}