using GoBike.Team.Repository.Models;
using GoBike.Team.Service.Models.Command;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace GoBike.Team.Service.Managers
{
    /// <summary>
    /// 車隊共用服務
    /// </summary>
    public class TeamCommonService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public TeamCommonService(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 取得流水號 ID
        /// </summary>
        /// <param name="createDate">createDate</param>
        /// <returns>string</returns>
        protected string GetSerialID(DateTime createDate)
        {
            return $"{Guid.NewGuid().ToString().Substring(0, 6)}-{createDate:yyyy}-{createDate:MMdd}";
        }

        /// <summary>
        /// 驗證車隊指令資料
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isVerifyExaminer">isVerifyExaminer</param>
        /// <param name="isVerifyTarget">isVerifyTarget</param>
        /// <param name="isVerifyTargets">isVerifyTargets</param>
        /// <param name="isVerifyTeamInfo">isVerifyTeamInfo</param>
        /// <param name="isVerifyAnnouncementInfo">isVerifyAnnouncementInfo</param>
        /// <param name="isVerifyEventInfo">isVerifyEventInfo</param>
        /// <returns>bool</returns>
        protected bool VerifyTeamCommand(TeamCommandDto teamCommand, bool isVerifyExaminer, bool isVerifyTarget, bool isVerifyTargets, bool isVerifyTeamInfo, bool isVerifyAnnouncementInfo, bool isVerifyEventInfo)
        {
            if (teamCommand == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(teamCommand.TeamID))
            {
                return false;
            }

            if (isVerifyExaminer)
            {
                if (string.IsNullOrEmpty(teamCommand.ExaminerID))
                {
                    return false;
                }
            }

            if (isVerifyTarget)
            {
                if (string.IsNullOrEmpty(teamCommand.TargetID))
                {
                    return false;
                }
            }

            if (isVerifyTargets)
            {
                if (teamCommand.TargetIDs == null || teamCommand.TargetIDs.Count() == 0)
                {
                    return false;
                }
            }

            if (isVerifyTeamInfo)
            {
                if (teamCommand.TeamInfo == null)
                {
                    return false;
                }
            }

            if (isVerifyAnnouncementInfo)
            {
                if (teamCommand.AnnouncementInfo == null)
                {
                    return false;
                }
            }

            if (isVerifyEventInfo)
            {
                if (teamCommand.EventInfo == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 驗證車隊審查者權限
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="examinerID">examinerID</param>
        /// <param name="isSupreme">isSupreme</param>
        /// <param name="isVerifyTarget">isVerifyTarget</param>
        /// <param name="targetID">targetID</param>
        /// <returns>bool</returns>
        protected bool VerifyTeamExaminerAuthority(TeamData teamData, string examinerID, bool isSupreme, bool isVerifyTarget, string targetID)
        {
            if (string.IsNullOrEmpty(examinerID))
            {
                return false;
            }

            if (isSupreme)
            {
                if (!examinerID.Equals(teamData.TeamLeaderID))
                {
                    return false;
                }
            }

            if (!teamData.TeamLeaderID.Equals(examinerID) && !teamData.TeamViceLeaderIDs.Contains(examinerID))
            {
                return false;
            }

            if (isVerifyTarget)
            {
                if (string.IsNullOrEmpty(targetID))
                {
                    return false;
                }

                if (examinerID.Equals(targetID))
                {
                    return false;
                }
                if (targetID.Equals(teamData.TeamLeaderID))
                {
                    return false;
                }
            }

            return true;
        }
    }
}