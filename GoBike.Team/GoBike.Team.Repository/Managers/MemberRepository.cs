﻿using GoBike.Team.Core.Applibs;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Repository.Managers
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
        /// 取得會員資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>MemberData</returns>
        public async Task<MemberData> GetMemebrData(string memberID)
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
    }
}