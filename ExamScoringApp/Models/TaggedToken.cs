using MongoDB.Bson.Serialization.Attributes;

namespace ExamScoringApp.Models
{
    public class TaggedToken
    {

        [BsonElement("token")]
        public string Token { get; set; }
        [BsonElement("lemma")]
        public string Lemma { get; set; }
        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("flag")]
        public string Flag { get; set; }

    }
}