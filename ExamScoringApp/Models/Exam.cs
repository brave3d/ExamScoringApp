using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamScoringApp.Models
{ 
    public class Exam
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("Course"),Required]
        public string Course { get; set; }

        [BsonElement("TeacherId")]
        public ObjectId TeacherId { get; set; }

        [BsonElement("Semester"), Required]
        public string Semester { get; set; }

        [BsonElement("Year"), Required]
        public string Year { get; set; }

    }
}