using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExamScoringApp.Models;
using ExamScoringApp.Helpers;

namespace ExamScoringApp.Controllers
{
    [Authorize]
    public class QuestionsController : CoreController
    {

        // GET: /Questions/
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string examId=null)
        {
          
            var questions = await Db.Questions.Find(s => true).ToListAsync();
            if (examId != null)
            {
                questions = questions.Where(q => q.ExamId == new ObjectId(examId)).ToList();
            }
            var questionVMs = new List<QuestionVM>();
            foreach (var item in questions)
            {
                questionVMs.Add(new QuestionVM
                {
                    Id = item.Id,
                    Text = item.Text.Truncate(180),
                    Points = item.Points,
                    Exam = GetExam(item.ExamId),
                });
            }
            ViewData["Exams"] = GetAllExams();
            return View(questionVMs.OrderBy(q=>q.Exam?.Course));
        }

        public Exam GetExam(ObjectId examId)
        {
            return Db.Exams.Find(e => e.Id == examId).FirstOrDefault();
        }

        public List<Exam> GetAllExams()
        {
            return Db.Exams.Find(e => true).ToList();
        }



        //GET: /Questions/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var exams = Db.Exams.Find(s => true).ToList();
            ViewData["Exams"] = exams;
            return View(new Question());
        }


        //[HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> SaveQuestion(string examId, string text, List<Blank> blanks,int points = 0, string questionId =null)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(examId) || !ObjectId.TryParse(examId, out Id))
            {
                return Json(new { success = false, responseText = "Please select an exam." }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(text))
            {
                return Json(new { success = false, responseText = "Question cannot be empty" }, JsonRequestBehavior.AllowGet);
            }

            var result = await Db.Exams.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return Json(new { success = false, responseText = "Exam not found, try again later." }, JsonRequestBehavior.AllowGet);
            }

            if (blanks == null)
            {
                return Json(new { success = false, responseText = "Please Add blanks to the question" }, JsonRequestBehavior.AllowGet);
            }
         
            var q = new Question
            {
                Text = text,
                Blanks = blanks,
                ExamId = Id,
                Points = points
            };
            if (questionId!=null)
            {
                var dbQuestion = Db.Questions.Find(a => a.Id == new ObjectId(questionId)).FirstOrDefault();
                dbQuestion.Text = q.Text;
                dbQuestion.ExamId = q.ExamId;
                dbQuestion.Points = q.Points;
                dbQuestion.Blanks.Clear();
                dbQuestion.Blanks.AddRange(q.Blanks);
                await Db.Questions.ReplaceOneAsync(t => t.Id == new ObjectId(questionId), dbQuestion, new UpdateOptions { IsUpsert = true });
            }
            else {
                await Db.Questions.InsertOneAsync(q);
            }
            return Json(new { success = true, responseText = "Question Saved!" }, JsonRequestBehavior.AllowGet);

        }


        private bool QuestionExistsInDb(Question q)
        {
            throw new NotImplementedException();
        }


        // POST: /Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                Db.Questions.InsertOne(question);
                return RedirectToAction("Index");
            }

            return View(question);
        }

        [Authorize(Roles ="Admin")]

        // GET: /Questions/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = await Db.Questions.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return HttpNotFound();
            }
            var exam = await Db.Exams.Find(t => t.Id == result.ExamId).FirstOrDefaultAsync();
            ViewData["Exam"] = exam;
            return View(result);
        }

        // GET: /Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = await Db.Questions.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return HttpNotFound();
            }
            var exams = Db.Exams.Find(s => true).ToList();
            ViewData["Exams"] = exams;
            return View(result);
        }

        // POST: /Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Question question)
        {
             question.Id = new ObjectId(id);
             await Db.Questions.ReplaceOneAsync(t => t.Id.Equals(question.Id), question, new UpdateOptions{ IsUpsert=true });
             return RedirectToAction("Index");
            //return View(question);
        }

        // GET: /Questions/Delete/5
        public async Task<ActionResult> Delete(string id, string examId=null)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = await Db.Questions.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return HttpNotFound();
            }
            var exam = await Db.Exams.Find(t => t.Id == result.ExamId).FirstOrDefaultAsync();
            ViewData["Exam"] = exam;

            return View(result);
        }

        // POST: /Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string examId = null)
        {
            await Db.Questions.DeleteOneAsync(t => t.Id == new ObjectId(id));
            return RedirectToAction("Index",new {examId = examId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}