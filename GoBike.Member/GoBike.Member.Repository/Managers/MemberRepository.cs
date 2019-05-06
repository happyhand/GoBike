using GoBike.Member.Core.Applibs;
using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Member.Repository.Managers
{
    /// <summary>
    /// 會員資料庫
    /// </summary>
    public class MemberRepository : IMemberRepository
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<MemberRepository> logger;

        /// <summary>
        /// memberDatas
        /// </summary>
        private readonly IMongoCollection<MemberData> memberDatas;

        /// <summary>
        /// serialNumberDatas
        /// </summary>
        private readonly IMongoCollection<SerialNumberData> serialNumberDatas;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public MemberRepository(ILogger<MemberRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.MemberDatabase);
            this.memberDatas = db.GetCollection<MemberData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Member);
            this.serialNumberDatas = db.GetCollection<SerialNumberData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.SerialNumber);
        }

        /// <summary>
        /// 建立會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateMemberData(MemberData memberData)
        {
            try
            {
                await this.memberDatas.InsertOneAsync(memberData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Member Data Error >>> Data:{JsonConvert.SerializeObject(memberData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除會員資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteMemebrData(string memberID)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("MemberID", memberID);
                DeleteResult result = await this.memberDatas.DeleteManyAsync(filter);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Memebr Data Error >>> MemberID:{memberID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得會員序號
        /// </summary>
        /// <returns>long</returns>
        public async Task<Tuple<long, string>> GetMemberSerialNumber()
        {
            try
            {
                long count = await this.serialNumberDatas.CountDocumentsAsync(new BsonDocument());
                if (count == 0)
                {
                    await this.serialNumberDatas.InsertOneAsync(new SerialNumberData() { SequenceName = "MemberID", SequenceValue = 100001 });
                }

                var filter = Builders<SerialNumberData>.Filter.Eq("SequenceName", "MemberID");
                var update = Builders<SerialNumberData>.Update.Inc("SequenceValue", 1);
                SerialNumberData result = await this.serialNumberDatas.FindOneAndUpdateAsync(filter, update);
                return Tuple.Create(result.SequenceValue, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Serial Number Error\n{ex}");
                return Tuple.Create<long, string>(-1, "取得會員序號發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員資料 (By Email)
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemebrDataByEmail(string email)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("Email", email);
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Data Error >>> Email:{email}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料 (By MemberID)
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemebrDataByMemberID(string memberID)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("MemberID", memberID);
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Data Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料 (By Mobile)
        /// </summary>
        /// <param name="mobile">mobile</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemebrDataByMobile(string mobile)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("Mobile", mobile);
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Data Error >>> mobile:{mobile}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>MemberDatas</returns>
        public async Task<IEnumerable<MemberData>> GetMemebrDataList(IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.In("MemberID", memberIDs);
                return await this.memberDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Data List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateMemebrData(MemberData memberData)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("_id", memberData.Id);
                ReplaceOneResult result = await this.memberDatas.ReplaceOneAsync(filter, memberData);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新會員資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "會員資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Memebr Data Error >>> Data:{JsonConvert.SerializeObject(memberData)}\n{ex}");
                return Tuple.Create(false, "更新會員資料發生錯誤.");
            }
        }

        /// <summary>
        /// 更新會員登入日期資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="loginDate">loginDate</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateMemebrLoginDate(string memberID, DateTime loginDate)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<MemberData> update = Builders<MemberData>.Update.Set(data => data.LoginDate, loginDate);
                UpdateResult result = await this.memberDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新會員登入日期資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "會員登入日期資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Memebr Login Date Error >>> MemberID:{memberID} LoginDate:{loginDate}\n{ex}");
                return Tuple.Create(false, "更新會員登入日期資料發生錯誤.");
            }
        }

        /// <summary>
        /// 驗證會員資料
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>bool</returns>
        public async Task<bool> VerifyMemberList(IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.In("MemberID", memberIDs);
                long count = await this.memberDatas.CountDocumentsAsync(filter);
                return memberIDs.Count() == count;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Verify Member List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return false;
            }
        }
    }
}