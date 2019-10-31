using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamScoringApp.Models
{
    [NotMapped]
    public class StudentsTextPointsVM
    {
        public int Id { get; set; }
        public string StudentNo { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Text { get; set; }
        public ObjectId TextId { get; set; }
        [UIHint("Points")]
        public double Average { get; set; }

    }
}