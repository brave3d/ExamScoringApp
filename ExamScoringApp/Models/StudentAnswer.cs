using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ExamScoringApp.Models
{
    public class StudentAnswer
    {

        [BsonElement("StudentId")]
        public ObjectId StudentId { get; set; }

        [BsonElement("AnswerTxt")]
        public ObjectId AnswerTxt { get; set; }

        [BsonElement("Score")]
        public int Score { get; set; }
    }
}