using GoBike.Member.Core.Applibs;
using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
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
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public MemberRepository(ILogger<MemberRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.MemberDatabase);
            this.memberDatas = db.GetCollection<MemberData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Member);
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

        ///// <summary>
        ///// 建立會員序號
        ///// </summary>
        ///// <returns>long</returns>
        //private async Task<string> CreateMemberSerialNumber()
        //{
        //    long count = await this.serialNumbers.CountDocumentsAsync(new BsonDocument());
        //    if (count == 0)
        //    {
        //        await this.serialNumbers.InsertOneAsync(new SerialNumber() { SequenceName = "MemberSerialNumber", SequenceValue = 10001 });
        //    }

        //    var filter = Builders<SerialNumber>.Filter.Eq("SequenceName", "MemberSerialNumber");
        //    var update = Builders<SerialNumber>.Update.Inc("SequenceValue", 1);
        //    SerialNumber result = await this.serialNumbers.FindOneAndUpdateAsync(filter, update);
        //    return result.SequenceValue;
        //}
    }
}