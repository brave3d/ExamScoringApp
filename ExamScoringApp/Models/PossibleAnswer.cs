using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ExamScoringApp.Models
{
    public class PossibleAnswer
    {
        [BsonElement("AnswerTxt")]
        public string AnswerTxt { get; set; }

        [BsonElement("Score")]
        public int Score { get; set; }
    }
}