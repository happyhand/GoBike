using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using GoBike.Member.Repository.Models.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.Repository.Managers
{
	public class MemberRepository : IMemberRepository
	{
		private readonly ILogger<MemberRepository> logger;
		private readonly IMongoCollection<MemberData> memberDatas;
		private readonly IMongoCollection<SerialNumber> serialNumbers;

		public MemberRepository(IConfiguration config, ILogger<MemberRepository> logger, IOptions<DBSetting> options)
		{
			this.logger = logger;
			IMongoClient client = new MongoClient(options.Value.ConnectionString);
			IMongoDatabase db = client.GetDatabase(options.Value.MemberDatabase);
			this.memberDatas = db.GetCollection<MemberData>("Member");
			this.serialNumbers = db.GetCollection<SerialNumber>("SerialNumber");
		}

		public async Task<Tuple<int, string>> CreateMember(MemberData memberData)
		{
			try
			{
				await this.memberDatas.InsertOneAsync(memberData);
				return Tuple.Create(1, "Create Member Success.");
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Create Member Error >>> ID:{memberData.ID} Account:{memberData.Account} Password:{memberData.Password}\n{ex}");
				return Tuple.Create(-999, "Create Member Error.");
			}
		}

		public async Task<MemberData> GetMemebrData(int id)
		{
			try
			{
				return await this.memberDatas.Find(memberData => memberData.ID == id).FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Get Memebr Data Error >>> ID:{id}\n{ex}");
				return null;
			}
		}

		public async Task<MemberData> GetMemebrData(string account)
		{
			try
			{
				return await this.memberDatas.Find(memberData => memberData.Account.Equals(account)).FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Get Memebr Data Error >>> Account:{account}\n{ex}");
				return null;
			}
		}

		public async Task<int> CreateMemberSerialNumber()
		{
			try
			{
				var filter = Builders<SerialNumber>.Filter.Eq("SequenceName", "MemberSerialNumber");
				var update = Builders<SerialNumber>.Update.Inc("SequenceValue", 1);
				SerialNumber result = await this.serialNumbers.FindOneAndUpdateAsync(filter, update);
				return result.SequenceValue;
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Create Memebr Serial Number Error\n{ex}");
				return -1;
			}
		}
	}
}