using GoBike.Service.Core.Applibs;
using GoBike.Service.Repository.Interface.Team;
using GoBike.Service.Repository.Models.Team;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Managers.Team
{
    /// <summary>
    /// 車隊資料庫
    /// </summary>
    public class TeamRepository : ITeamRepository
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<TeamRepository> logger;

        /// <summary>
        /// teamInteractiveDatas
        /// </summary>
        private readonly IMongoCollection<TeamAnnouncementData> teamAnnouncementDatas;

        /// <summary>
        /// teamDatas
        /// </summary>
        private readonly IMongoCollection<TeamData> teamDatas;

        /// <summary>
        /// teamInteractiveDatas
        /// </summary>
        private readonly IMongoCollection<TeamInteractiveData> teamInteractiveDatas;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public TeamRepository(ILogger<TeamRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.TeamDatabase);
            this.teamDatas = db.GetCollection<TeamData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Team);
            this.teamInteractiveDatas = db.GetCollection<TeamInteractiveData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.TeamInteractive);
            this.teamAnnouncementDatas = db.GetCollection<TeamAnnouncementData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.TeamAnnouncement);
        }

        #region 車隊資料

        /// <summary>
        /// 建立車隊資料
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateTeamData(TeamData teamData)
        {
            try
            {
                await this.teamDatas.InsertOneAsync(teamData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Data Error >>> Data:{JsonConvert.SerializeObject(teamData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除車隊資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteTeamData(string teamID)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                DeleteResult result = await this.teamDatas.DeleteManyAsync(filter);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Delete Team Data Fail For IsAcknowledged >>> TeamID:{teamID}");
                    return false;
                }

                if (result.DeletedCount == 0)
                {
                    this.logger.LogError($"Delete Team Data Fail For DeletedCount >>> TeamID:{teamID}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Data Error >>> TeamID:{teamID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>TeamData</returns>
        public async Task<TeamData> GetTeamData(string teamID)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                return await this.teamDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data Error >>> TeamID:{teamID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得車隊列表資料 (By CityID)
        /// </summary>
        /// <param name="cityID">cityID</param>
        /// <returns>TeamDatas</returns>
        public async Task<IEnumerable<TeamData>> GetTeamDataListByCityID(int cityID)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("CityID", cityID);
                return await this.teamDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List By CityID Error >>> CityID:{cityID}\n{ex}");
                return new List<TeamData>();
            }
        }

        /// <summary>
        /// 取得車隊列表資料 (By CreateDate)
        /// </summary>
        /// <param name="createDate">createDate</param>
        /// <returns>TeamDatas</returns>
        public async Task<IEnumerable<TeamData>> GetTeamDataListByCreateDate(TimeSpan timeSpan)
        {
            try
            {
                return await this.teamDatas.Find(data => (data.CreateDate - DateTime.Now) <= timeSpan).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List By CreateDate Error >>> TimeSpan:{timeSpan}\n{ex}");
                return new List<TeamData>();
            }
        }

        /// <summary>
        /// 取得車隊列表資料 (By TeamID)
        /// </summary>
        /// <param name="teamIDs">teamIDs</param>
        /// <returns>TeamDatas</returns>
        public async Task<IEnumerable<TeamData>> GetTeamDataListByTeamID(IEnumerable<string> teamIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.In("TeamID", teamIDs);
                return await this.teamDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List By TeamID Error >>> TeamIDs:{JsonConvert.SerializeObject(teamIDs)}\n{ex}");
                return new List<TeamData>();
            }
        }

        /// <summary>
        /// 取得車隊資料列表 (By TeamName)
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <param name="isStrict">isStrict</param>
        /// <returns>TeamDatas</returns>
        public async Task<IEnumerable<TeamData>> GetTeamDataListByTeamName(string teamName, bool isStrict)
        {
            try
            {
                if (isStrict)
                {
                    return await this.teamDatas.Find(data => data.TeamName.Equals(teamName)).ToListAsync();
                }

                return await this.teamDatas.Find(data => data.TeamName.Contains(teamName)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List By TeamName Error >>> TeamName:{teamName} IsStrict:{isStrict}\n{ex}");
                return new List<TeamData>();
            }
        }

        /// <summary>
        /// 取得會員的車隊列表資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>TeamDatas</returns>
        public async Task<IEnumerable<TeamData>> GetTeamDataListOfMember(string memberID)
        {
            try
            {
                TeamData teamData = await this.teamDatas.Find(data => data.TeamLeaderID.Equals(memberID)).FirstOrDefaultAsync();
                IEnumerable<TeamData> teamDataList = await this.teamDatas.Find(data => data.TeamMemberIDs.Contains(memberID)).ToListAsync();
                return teamDataList.Prepend(teamData);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List Of Member Error >>> MemberID:{memberID}\n{ex}");
                return new List<TeamData>();
            }
        }

        /// <summary>
        /// 更新車隊資料
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateTeamData(TeamData teamData)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("_id", teamData.Id);
                ReplaceOneResult result = await this.teamDatas.ReplaceOneAsync(filter, teamData);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Team Data Fail For IsAcknowledged >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Team Data Fail For ModifiedCount >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Data Error >>> Data:{JsonConvert.SerializeObject(teamData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 驗證車隊資料 (By TeamLeaderID)
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <returns>bool</returns>
        public async Task<bool> VerifyTeamDataByTeamLeaderID(string memberID)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamLeaderID", memberID);
                return await this.teamDatas.CountDocumentsAsync(filter) > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Verify Team Data By Team Leader ID Error >>> MemberID:{memberID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 驗證車隊資料 (By TeamName)
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <returns>bool</returns>
        public async Task<bool> VerifyTeamDataByTeamName(string teamName)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamName", teamName);
                return await this.teamDatas.CountDocumentsAsync(filter) > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Verify Team Data By Team Name Error >>> TeamName:{teamName}\n{ex}");
                return false;
            }
        }

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 建立多筆車隊互動資料
        /// </summary>
        /// <param name="teamInteractiveDatas">teamInteractiveDatas</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateManyTeamInteractiveData(IEnumerable<TeamInteractiveData> teamInteractiveDatas)
        {
            try
            {
                await this.teamInteractiveDatas.InsertManyAsync(teamInteractiveDatas);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Many Team Interactive Data Error >>> Data:{JsonConvert.SerializeObject(teamInteractiveDatas)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 建立車隊互動資料
        /// </summary>
        /// <param name="teamInteractiveData">teamInteractiveData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateTeamInteractiveData(TeamInteractiveData teamInteractiveData)
        {
            try
            {
                await this.teamInteractiveDatas.InsertOneAsync(teamInteractiveData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Interactive Data Error >>> Data:{JsonConvert.SerializeObject(teamInteractiveData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除車隊互動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberID">memberID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteTeamInteractiveData(string teamID, string memberID)
        {
            try
            {
                DeleteResult result = await this.teamInteractiveDatas.DeleteManyAsync(data => data.TeamID.Equals(teamID) && data.MemberID.Equals(memberID));
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Delete Review Join Team Data Fail For IsAcknowledged >>> TeamID:{teamID} MemberID:{memberID}");
                    return false;
                }

                if (result.DeletedCount == 0)
                {
                    this.logger.LogError($"Delete Review Join Team Data For DeletedCount >>> TeamID:{teamID} MemberID:{memberID}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Review Join Team Data Error >>> TeamID:{teamID} MemberID:{memberID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得指定的車隊互動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberID">memberID</param>
        /// <returns>TeamInteractiveData</returns>
        public async Task<TeamInteractiveData> GetAppointTeamInteractiveData(string teamID, string memberID)
        {
            try
            {
                return await this.teamInteractiveDatas.Find(data => data.TeamID.Equals(teamID) && data.MemberID.Equals(memberID)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Interactive Data Of Team Error >>> TeamID:{teamID} MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員的車隊互動資料列表資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>TeamInteractiveDatas</returns>
        public async Task<IEnumerable<TeamInteractiveData>> GetTeamInteractiveDataListOfMember(string memberID)
        {
            try
            {
                return await this.teamInteractiveDatas.Find(data => data.MemberID.Equals(memberID)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Interactive Data List Of Member Error >>> MemberID:{memberID}\n{ex}");
                return new List<TeamInteractiveData>();
            }
        }

        /// <summary>
        /// 取得車隊的車隊互動資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>TeamInteractiveDatas</returns>
        public async Task<IEnumerable<TeamInteractiveData>> GetTeamInteractiveDataListOfTeam(string teamID)
        {
            try
            {
                return await this.teamInteractiveDatas.Find(data => data.TeamID.Equals(teamID)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Interactive Data List Of Team Error >>> TeamID:{teamID}\n{ex}");
                return new List<TeamInteractiveData>();
            }
        }

        /// <summary>
        /// 更新車隊互動資料
        /// </summary>
        /// <param name="teamInteractiveData">teamInteractiveData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateTeamInteractiveData(TeamInteractiveData teamInteractiveData)
        {
            try
            {
                FilterDefinition<TeamInteractiveData> filter = Builders<TeamInteractiveData>.Filter.Eq("_id", teamInteractiveData.Id);
                ReplaceOneResult result = await this.teamInteractiveDatas.ReplaceOneAsync(filter, teamInteractiveData);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Team Interactive Data Fail For IsAcknowledged >>> Data:{JsonConvert.SerializeObject(teamInteractiveData)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Team Interactive Data Fail For ModifiedCount >>> Data:{JsonConvert.SerializeObject(teamInteractiveData)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Interactive Data Error >>> Data:{JsonConvert.SerializeObject(teamInteractiveData)}\n{ex}");
                return false;
            }
        }

        #endregion 車隊互動資料

        #region 車隊公告資料

        /// <summary>
        /// 建立車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementData">teamAnnouncementData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateTeamAnnouncementData(TeamAnnouncementData teamAnnouncementData)
        {
            try
            {
                await this.teamAnnouncementDatas.InsertOneAsync(teamAnnouncementData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除車隊資料
        /// </summary>
        /// <param name="announcementID">announcementID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteTeamAnnouncementData(string announcementID)
        {
            try
            {
                FilterDefinition<TeamAnnouncementData> filter = Builders<TeamAnnouncementData>.Filter.Eq("AnnouncementID", announcementID);
                DeleteResult result = await this.teamAnnouncementDatas.DeleteManyAsync(filter);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Delete Team Announcement Data Fail For IsAcknowledged >>> AnnouncementID:{announcementID}");
                    return false;
                }

                if (result.DeletedCount == 0)
                {
                    this.logger.LogError($"Delete Team Announcement Data Fail For DeletedCount >>> AnnouncementID:{announcementID}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Announcement Data Error >>> AnnouncementID:{announcementID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得公告資料
        /// </summary>
        /// <param name="announcementID">announcementID</param>
        /// <returns>AnnouncementData</returns>
        public async Task<TeamAnnouncementData> GetTeamAnnouncementData(string announcementID)
        {
            try
            {
                FilterDefinition<TeamAnnouncementData> filter = Builders<TeamAnnouncementData>.Filter.Eq("AnnouncementID", announcementID);
                return await this.teamAnnouncementDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Announcement Data Error >>> AnnouncementID:{announcementID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得車隊公告資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>AnnouncementDatas</returns>
        public async Task<IEnumerable<TeamAnnouncementData>> GetTeamAnnouncementDataListOfTeam(string teamID)
        {
            try
            {
                FilterDefinition<TeamAnnouncementData> filter = Builders<TeamAnnouncementData>.Filter.Eq("TeamID", teamID);
                return await this.teamAnnouncementDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get TeamAnnouncement Data List Of Team Error >>> TeamID:{teamID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementData">teamAnnouncementData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateTeamAnnouncementData(TeamAnnouncementData teamAnnouncementData)
        {
            try
            {
                FilterDefinition<TeamAnnouncementData> filter = Builders<TeamAnnouncementData>.Filter.Eq("_id", teamAnnouncementData.Id);
                ReplaceOneResult result = await this.teamAnnouncementDatas.ReplaceOneAsync(filter, teamAnnouncementData);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Team Announcement Data Fail For IsAcknowledged >>> Data:{JsonConvert.SerializeObject(teamAnnouncementData)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Team Announcement Data Fail For ModifiedCount >>> Data:{JsonConvert.SerializeObject(teamAnnouncementData)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementData)}\n{ex}");
                return false;
            }
        }

        #endregion 車隊公告資料
    }
}