using GoBike.Team.Core.Applibs;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Team.Repository.Managers
{
    /// <summary>
    /// 車隊資料庫
    /// </summary>
    public class TeamRepository : ITeamRepository
    {
        /// <summary>
        /// interactiveDatas
        /// </summary>
        private readonly IMongoCollection<InteractiveData> interactiveDatas;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<TeamRepository> logger;

        /// <summary>
        /// teamDatas
        /// </summary>
        private readonly IMongoCollection<TeamData> teamDatas;

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
            this.interactiveDatas = db.GetCollection<InteractiveData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Interactive);
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
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Data Error >>> TeamID:{teamID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得車隊資料 (By TeamCreatorID)
        /// </summary>
        /// <param name="teamCreatorID">teamCreatorID</param>
        /// <returns>TeamData</returns>
        public async Task<TeamData> GetTeamDataByTeamCreatorID(string teamCreatorID)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamCreatorID", teamCreatorID);
                return await this.teamDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data Error >>> TeamCreatorID:{teamCreatorID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得車隊資料 (By TeamID)
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>TeamData</returns>
        public async Task<TeamData> GetTeamDataByTeamID(string teamID)
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
        /// 取得車隊列表資料
        /// </summary>
        /// <param name="teamIDs">teamIDs</param>
        /// <returns>TeamDatas</returns>
        public async Task<IEnumerable<TeamData>> GetTeamDataList(IEnumerable<string> teamIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.In("TeamID", teamIDs);
                return await this.teamDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List Error >>> TeamIDs:{JsonConvert.SerializeObject(teamIDs)}\n{ex}");
                return null;
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
                return await this.teamDatas.Find(data => data.TeamCreatorID.Equals(memberID) || data.TeamPlayerIDs.Contains(memberID)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List Of Member Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新車隊黑名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="blacklistIDs">blacklistIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamBlacklistData(string teamID, IEnumerable<string> blacklistIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                UpdateDefinition<TeamData> update = Builders<TeamData>.Update.Set(data => data.TeamBlacklistIDs, blacklistIDs);
                UpdateResult result = await this.teamDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新車隊黑名單資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "車隊黑名單資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Data Error >>> TeamID:{teamID} BlacklistIDs:{JsonConvert.SerializeObject(blacklistIDs)}\n{ex}");
                return Tuple.Create(false, "更新車隊黑名單資料發生錯誤.");
            }
        }

        /// <summary>
        /// 更新車隊被列入黑名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamBlacklistedData(string teamID, IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                UpdateDefinition<TeamData> update = Builders<TeamData>.Update.Set(data => data.TeamBlacklistedIDs, memberIDs);
                UpdateResult result = await this.teamDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新車隊被列入黑名單資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "車隊被列入黑名單資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Data Error >>> TeamID:{teamID} MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return Tuple.Create(false, "更新車隊被列入黑名單資料發生錯誤.");
            }
        }

        /// <summary>
        /// 更新車隊資料
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamData(TeamData teamData)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("_id", teamData.Id);
                ReplaceOneResult result = await this.teamDatas.ReplaceOneAsync(filter, teamData);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新車隊資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "車隊資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Data Error >>> Data:{JsonConvert.SerializeObject(teamData)}\n{ex}");
                return Tuple.Create(false, "更新車隊資料發生錯誤.");
            }
        }

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="viceLeaderID">viceLeaderID</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamViceLeader(string teamID, string viceLeaderID)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                UpdateDefinition<TeamData> update = Builders<TeamData>.Update.Set(data => data.TeamViceLeaderID, viceLeaderID);
                UpdateResult result = await this.teamDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新車隊副隊長.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "車隊副隊長未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Vice Leader Error >>> TeamID:{teamID} ViceLeaderID:{viceLeaderID}\n{ex}");
                return Tuple.Create(false, "更新車隊副隊長發生錯誤.");
            }
        }

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 建立車隊互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateTeamInteractiveData(InteractiveData interactiveData)
        {
            try
            {
                await this.interactiveDatas.InsertOneAsync(interactiveData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Interactive Data Error >>> Data:{JsonConvert.SerializeObject(interactiveData)}\n{ex}");
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
                DeleteResult result = await this.interactiveDatas.DeleteManyAsync(data => data.TeamID.Equals(teamID) && data.MemberID.Equals(memberID));
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Interactive Data Error >>> TeamID:{teamID} MemberID:{memberID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得車隊指定互動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveData</returns>
        public async Task<InteractiveData> GetTeamInteractiveData(string teamID, string memberID)
        {
            try
            {
                return await this.interactiveDatas.Find(data => data.TeamID.Equals(teamID) && data.MemberID.Equals(memberID)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Interactive Data Error >>> TeamID:{teamID} MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得申請加入互動資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<IEnumerable<InteractiveData>> GetTeamInteractiveDataListForApplyForJoin(string teamID)
        {
            try
            {
                int applyForJoinStatus = (int)InteractiveStatusType.ApplyForJoin;
                return await this.interactiveDatas.Find(data => data.TeamID.Equals(teamID) && data.Status == applyForJoinStatus).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Interactive Data List For Apply For Join Error >>> TeamID:{teamID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得邀請加入互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<IEnumerable<InteractiveData>> GetTeamInteractiveDataListForInviteJoin(string memberID)
        {
            try
            {
                int inviteJoinStatus = (int)InteractiveStatusType.InviteJoin;
                return await this.interactiveDatas.Find(data => data.MemberID.Equals(memberID) && data.Status == inviteJoinStatus).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Interactive Data List For Invite Join Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        #endregion 車隊互動資料
    }
}