using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExamScoringApp.Models;
using ExamScoringApp.Models.Base;

namespace ExamScoringApp.Controllers
{
    [Authorize]
    public class HomeController : CoreController
    {
        public async Task<ActionResult> Index()
        {

            var stats = new Stats()
            {
                Teachers = Db.Users.CountDocuments(u =>  u.Roles.Any(r => r == "Admin")),
                Students = Db.Users.CountDocuments(u => !u.Roles.Any(r => r == "Admin")),
                Exams = Db.Exams.CountDocuments(u => true),
                Questions = Db.Questions.CountDocuments(u => true)
        };
            var questions = Db.Questions.Find(q => true).ToList();
            stats.Answers = 0;
            questions.ForEach(q =>
            {
                q.Blanks.ForEach(b => {
                    stats.Answers += b.PossibleAnswers != null ? b.PossibleAnswers.Count : 0;
                    stats.Answers += b.StudentAnswers!=null ? b.StudentAnswers.Count : 0;
                });
            });
            return View(stats);
        }

        //public JsonResult GetSentences()
        //{
        //    var mdb = new DataAccess();
        //    var exams = mdb.GetTexts();
        //    return Json(exams, JsonRequestBehavior.AllowGet);
        //}

        

        public ActionResult Minor()
        {
            ViewData["SubTitle"] = "Simple example of second view";
            ViewData["Message"] = "Data are passing to view by ViewData from controller";

            return View();
        }
    }
}