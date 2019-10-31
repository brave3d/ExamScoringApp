using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ExamScoringApp.Models
{
    public class Blank
    {
        [BsonElement("Index")]
        public int Index { get; set; }

        [BsonElement("PossibleAnswers")]
        public virtual List<PossibleAnswer> PossibleAnswers { get; set; }

        [BsonElement("StudentAnswers")]
        public virtual List<StudentAnswer> StudentAnswers { get; set; }
    }
}