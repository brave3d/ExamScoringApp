using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamScoringApp.Models
{
    [NotMapped]
    public class QuestionVM:Question
    {
        public Exam Exam { get; set; }
    }
}