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
                return null;
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
                return null;
            }
        }

        /// <summary>
        /// 取得會員的邀請加入車隊列表資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>TeamDatas</returns>
        public async Task<IEnumerable<TeamData>> GetTeamDataListOfInviteJoin(string memberID)
        {
            try
            {
                return await this.teamDatas.Find(data => data.TeamInviteJoinIDs.Contains(memberID)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List Of Invite Join Error >>> MemberID:{memberID}\n{ex}");
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
                return await this.teamDatas.Find(data => data.TeamLeaderID.Equals(memberID) || data.TeamPlayerIDs.Contains(memberID)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List Of Member Error >>> MemberID:{memberID}\n{ex}");
                return null;
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
        /// 更新車隊副隊長群
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="teamViceLeaderIDs">teamViceLeaderIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamViceLeaders(string teamID, IEnumerable<string> teamViceLeaderIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                UpdateDefinition<TeamData> update = Builders<TeamData>.Update.Set(data => data.TeamViceLeaderIDs, teamViceLeaderIDs);
                UpdateResult result = await this.teamDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新車隊副隊長群.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "車隊副隊長群未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Vice Leaders Error >>> TeamID:{teamID} ViceLeaderIDs:{JsonConvert.SerializeObject(teamViceLeaderIDs)}\n{ex}");
                return Tuple.Create(false, "更新車隊副隊長群發生錯誤.");
            }
        }

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 更新申請加入名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamApplyForJoinIDs(string teamID, IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                UpdateDefinition<TeamData> update = Builders<TeamData>.Update.Set(data => data.TeamApplyForJoinIDs, memberIDs);
                UpdateResult result = await this.teamDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新申請加入名單資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "申請加入名單資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Apply For Join IDs Error >>> TeamID:{teamID} MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return Tuple.Create(false, "更新申請加入名單資料發生錯誤.");
            }
        }

        /// <summary>
        /// 更新車隊黑名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamBlacklistData(string teamID, IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                UpdateDefinition<TeamData> update = Builders<TeamData>.Update.Set(data => data.TeamBlacklistIDs, memberIDs);
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
                this.logger.LogError($"Update Team Data Error >>> TeamID:{teamID} MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
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
        /// 更新邀請加入名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateTeamInviteJoinIDs(string teamID, IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<TeamData> filter = Builders<TeamData>.Filter.Eq("TeamID", teamID);
                UpdateDefinition<TeamData> update = Builders<TeamData>.Update.Set(data => data.TeamInviteJoinIDs, memberIDs);
                UpdateResult result = await this.teamDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新邀請加入名單資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "邀請加入名單資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Invite Join IDs Error >>> TeamID:{teamID} MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return Tuple.Create(false, "更新邀請加入名單資料發生錯誤.");
            }
        }

        #endregion 車隊互動資料
    }
}