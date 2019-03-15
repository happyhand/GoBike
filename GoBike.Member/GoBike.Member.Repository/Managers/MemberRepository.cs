using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using GoBike.Member.Repository.Models.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace GoBike.Member.Repository.Managers
{
	public class MemberRepository : IMemberRepository
	{
		private const string MemberCollectionFlag = "Member";
		private const string SerialNumberCollectionFlag = "SerialNumber";
		private const string SerialNumberFlag = "MemberSerialNumber";

		private readonly ILogger<MemberRepository> logger;
		private readonly IMongoCollection<MemberData> memberDatas;
		private readonly IMongoCollection<SerialNumber> serialNumbers;

		public MemberRepository(IConfiguration config, ILogger<MemberRepository> logger, IOptions<DBSetting> options)
		{
			this.logger = logger;
			IMongoClient client = new MongoClient(options.Value.ConnectionString);
			IMongoDatabase db = client.GetDatabase(options.Value.MemberDatabase);
			this.memberDatas = db.GetCollection<MemberData>(MemberCollectionFlag);
			this.serialNumbers = db.GetCollection<SerialNumber>(SerialNumberCollectionFlag);
		}

		public async Task<bool> CreateMember(MemberData memberData)
		{
			try
			{
				await this.memberDatas.InsertOneAsync(memberData);
				return true;
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Create Member Error >>> ID:{memberData.ID} Account:{memberData.Account} Password:{memberData.Password}\n{ex}");
				return false;
			}
		}

		public async Task<int> CreateMemberSerialNumber()
		{
			var filter = Builders<SerialNumber>.Filter.Eq("SequenceName", SerialNumberFlag);
			var update = Builders<SerialNumber>.Update.Inc("SequenceValue", 1);
			try
			{
				SerialNumber result = await this.serialNumbers.FindOneAndUpdateAsync(filter, update);
				return result.SequenceValue;
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Create Memebr Serial Number Error\n{ex}");
				return -1;
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

		public async Task<MemberData> GetMemebrData(string account, string password)
		{
			try
			{
				return await this.memberDatas.Find(memberData => memberData.Account.Equals(account) && memberData.Password.Equals(password)).FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Get Memebr Data Error >>> Account:{account} Password:{password}\n{ex}");
				return null;
			}
		}

		public async Task<bool> UpdateMemebrData(MemberData memberData)
		{
			try
			{
				ReplaceOneResult result = await this.memberDatas.ReplaceOneAsync(option => option.ID == memberData.ID, memberData, new UpdateOptions { IsUpsert = true });
				return result.IsAcknowledged && result.ModifiedCount > 0;
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Update Memebr Data Error >>> ID:{memberData.ID}\n{ex}");
				return false;
			}
		}

		public async Task<bool> DeleteMemebrData(int id)
		{
			try
			{
				DeleteResult result = await this.memberDatas.DeleteManyAsync(option => option.ID == id).;
				return result.IsAcknowledged && result.DeletedCount > 0;
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Update Memebr Data Error >>> ID:{id}\n{ex}");
				return false;
			}
		}
	}
}