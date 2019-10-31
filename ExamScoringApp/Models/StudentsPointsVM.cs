using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamScoringApp.Models
{
    [NotMapped]
    public class StudentsPointsVM
    {
        public int Id { get; set; }
        public string StudentNo { get; set; }
        public string StudentId { get; set; }

        public string StudentName { get; set; }
        public string CorrectAnswers { get; set; }
        public string AllAnswered { get; set; }
        [UIHint("Points")]
        public double Points { get; set; }
        public ObjectId QuestionId { get; set; }
        public ObjectId ExamId { get; set; }


    }
}