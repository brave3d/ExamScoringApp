using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamScoringApp.Models
{
    public class ExamVM
    {
       
        public Exam Exam { get; set; }
        public List<Question> Questions { get; set; }
        public bool AnsweredByStudent { get; set; }
    }
}