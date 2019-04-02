using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using GoBike.Member.Repository.Models.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
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
        /// MemberCollectionFlag
        /// </summary>
        private const string MemberCollectionFlag = "Member";

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
        /// <param name="options">options</param>
        public MemberRepository(ILogger<MemberRepository> logger, IOptions<MongoDBSetting> options)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(options.Value.ConnectionString);
            IMongoDatabase db = client.GetDatabase(options.Value.MemberDatabase);
            this.memberDatas = db.GetCollection<MemberData>(MemberCollectionFlag);
        }

        /// <summary>
        /// 建立會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateMember(MemberData memberData)
        {
            try
            {
                await this.memberDatas.InsertOneAsync(memberData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Member Error >>> MemberID:{memberData.MemberID} Email:{memberData.Email} Password:{memberData.Password}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除會員資料
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteMemebrData(string id)
        {
            try
            {
                var filter = Builders<MemberData>.Filter.Eq("_id", ObjectId.Parse(id));
                DeleteResult result = await this.memberDatas.DeleteManyAsync(filter);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Memebr Data Error >>> ID:{id}\n{ex}");
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
                var filter = Builders<MemberData>.Filter.Eq("Email", email);
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Data Error >>> Email:{email}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料 (By Id)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemebrDataByID(string id)
        {
            try
            {
                var filter = Builders<MemberData>.Filter.Eq("_id", ObjectId.Parse(id));
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Data Error >>> ID:{id}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateMemebrData(MemberData memberData)
        {
            try
            {
                var filter = Builders<MemberData>.Filter.Eq("_id", memberData.Id);
                ReplaceOneResult result = await this.memberDatas.ReplaceOneAsync(filter, memberData);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Memebr Data Error >>> ID:{memberData.Id}\n{ex}");
                return false;
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