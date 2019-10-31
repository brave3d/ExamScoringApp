using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamScoringApp.Models
{
    [NotMapped]
    public class QuestionVM
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }

        public ObjectId ExamId { get; set; }
        public List<Answer> Answers { get; set; }
        public bool AnsweredByAdmin { get; set; }
        public bool AnsweredByMe { get; set; }


    }
}