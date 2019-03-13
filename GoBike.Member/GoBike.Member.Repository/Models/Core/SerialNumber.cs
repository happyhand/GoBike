using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Member.Repository.Models.Core
{
	public class SerialNumber
	{
		public ObjectId Id { get; set; }

		[BsonElement("SequenceName")]
		public string SequenceName { get; set; }

		[BsonElement("SequenceValue")]
		public int SequenceValue { get; set; }
	}
}