using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ExamScoringApp.Models
{
    public class Question
    {
        [BsonElement("_id"),UIHint("ObjectId")]
        public ObjectId Id { get; set; }

        [BsonElement("Question"), Display(Name = "Question"),Required]
        public string Text { get; set; }

        [BsonElement("IsAnsweredByTeacher")]
        public bool IsAnsweredByTeacher =>  Blanks!=null && Blanks.All(a => a.PossibleAnswers!=null);

        [BsonElement("ExamId"),Display(Name ="Exam"), UIHint("ObjectId")]
        public ObjectId ExamId { get; set; }

        [BsonElement("Blanks")]
        public List<Blank> Blanks { get; set; }

      

    }

    
}