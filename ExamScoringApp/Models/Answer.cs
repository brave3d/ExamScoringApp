using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ExamScoringApp.Models
{
    public class Answer
    {
        [BsonElement("UserId")]
        public ObjectId UserId { get; set; }

        [BsonElement("PossibleAnswers")]
        public virtual List<string> PossibleAnswers { get; set; }
    }
}