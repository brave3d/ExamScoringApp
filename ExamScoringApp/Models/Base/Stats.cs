using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamScoringApp.Models.Base
{
    public class Stats
    {
        public long Teachers { get; set; }
        public long  Students { get; set; }
        public long Exams { get; set; }
        public long Questions { get; set; }
        public long Answers { get; set; }

    }
}