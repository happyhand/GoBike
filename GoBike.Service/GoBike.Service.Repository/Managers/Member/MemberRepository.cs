using Firebase.Database;
using GoBike.Service.Core.Applibs;
using GoBike.Service.Repository.Interface.Member;
using GoBike.Service.Repository.Models;
using GoBike.Service.Repository.Models.Member;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Managers.Member
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

        #region firebase

        private FirebaseClient Firebase { get; }

        #endregion firebase

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

            //// firebase
            //var auth = "lhZwwaM0vNUkmGicMu5lyCNB8Aze5hJZQdbUQ4FS";
            //var baseUrl = "https://gobike-2019.firebaseio.com/";
            //var option = new FirebaseOptions()
            //{
            //    AuthTokenAsyncFactory = () => Task.FromResult(auth)
            //};
            //this.Firebase = new FirebaseClient(baseUrl, option);
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
                long count = await this.serialNumberDatas.CountDocumentsAsync(new BsonDocument());
                if (count == 0)
                {
                    await this.serialNumberDatas.InsertOneAsync(new SerialNumberData() { SequenceName = "MemberID", SequenceValue = 100001 });
                }

                FilterDefinition<SerialNumberData> filter = Builders<SerialNumberData>.Filter.Eq("SequenceName", "MemberID");
                UpdateDefinition<SerialNumberData> update = Builders<SerialNumberData>.Update.Inc("SequenceValue", 1);
                SerialNumberData result = await this.serialNumberDatas.FindOneAndUpdateAsync(filter, update);
                memberData.MemberID = result.SequenceValue.ToString();
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
        /// 取得會員資料 (By Email)
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemberDataByEmail(string email)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("Email", email);

                //var result = await this.Firebase
                //.Child("Member")
                //.PostAsync(JsonConvert.SerializeObject(new Member()
                //{
                //    Name = "123"
                //}));

                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Data Error >>> Email:{email}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料 (By FBToken)
        /// </summary>
        /// <param name="fbToken">fbToken</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemberDataByFBToken(string fbToken)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("FBToken", fbToken);
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Data Error >>> FBToken:{fbToken}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料 (By GoogleToken)
        /// </summary>
        /// <param name="googleToken">googleToken</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemberDataByGoogleToken(string googleToken)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("GoogleToken", googleToken);
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Data Error >>> GoogleToken:{googleToken}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料 (By MemberID)
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemberDataByMemberID(string memberID)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("MemberID", memberID);
                return await this.memberDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Data Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員資料列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>MemberDatas</returns>
        public async Task<IEnumerable<MemberData>> GetMemberDataList(IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.In("MemberID", memberIDs);
                return await this.memberDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Data List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新會員資料
        /// </summary>
        /// <param name="memberData">memberData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateMemberData(MemberData memberData)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("_id", memberData.Id);
                ReplaceOneResult result = await this.memberDatas.ReplaceOneAsync(filter, memberData);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Member Data Fail For IsAcknowledged >>> Data:{JsonConvert.SerializeObject(memberData)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Member Data Fail For ModifiedCount >>> Data:{JsonConvert.SerializeObject(memberData)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Member Data Error >>> Data:{JsonConvert.SerializeObject(memberData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 更新會員登入日期資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="loginDate">loginDate</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateMemberLoginDate(string memberID, DateTime loginDate)
        {
            try
            {
                FilterDefinition<MemberData> filter = Builders<MemberData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<MemberData> update = Builders<MemberData>.Update.Set(data => data.LoginDate, loginDate);
                UpdateResult result = await this.memberDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Member Login Date Fail For IsAcknowledged >>> MemberID:{memberID} LoginDate:{loginDate.ToString()}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Member Login Date Fail For ModifiedCount >>> MemberID:{memberID} LoginDate:{loginDate.ToString()}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Member Login Date Error >>> MemberID:{memberID} LoginDate:{loginDate}\n{ex}");
                return false;
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

        public class Member
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Tag")]
            public string Tag { get; set; }
        }
    }
}