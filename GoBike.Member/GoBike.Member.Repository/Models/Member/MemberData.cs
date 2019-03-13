using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Member.Repository.Models
{
	public class MemberData
	{
		public ObjectId Id { get; set; }

		[BsonElement("ID")]
		public int ID { get; set; }

		[BsonElement("Account")]
		public string Account { get; set; }

		[BsonElement("Password")]
		public string Password { get; set; }
	}
}