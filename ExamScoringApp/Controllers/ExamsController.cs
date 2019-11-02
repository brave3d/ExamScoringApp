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

namespace ExamScoringApp.Controllers
{
    [Authorize]
    public class ExamsController : CoreController
    {

        // GET: /Exams/
        public async Task<ActionResult> Index()
        {
            var exams = await Db.Exams.Find(s => true).ToListAsync();
            var examsVMs = new List<ExamVM>();
            foreach (var item in exams)
            {
                examsVMs.Add(new ExamVM {
                Exam = item,
                AnsweredByStudent= AnsweredByStudent(item.Id),
                Questions = Db.Questions.Find(q => q.ExamId.Equals(item.Id)).ToList()
            });
            }
            return View(examsVMs);
        }

        private bool AnsweredByStudent(ObjectId examId)
        {
            var questions = Db.Questions.Find(q => q.ExamId == examId).ToList();
            var userId = User.Identity.GetUserId();
            return questions.Any(q => q.Blanks != null && q.Blanks.Any(b => b.StudentAnswers != null && b.StudentAnswers.Any(sa => sa.StudentId == new ObjectId(userId))));
        }

        public ActionResult DoExam(string id)
        {
            var examVM = new ExamVM();
            var exam = Db.Exams.Find(e => new ObjectId(id) == e.Id).FirstOrDefault();
            if (exam == null)
            {
                return new HttpNotFoundResult();
            }
            var questions = Db.Questions.Find(q=>q.ExamId == new ObjectId(id)).ToList();
            examVM.Exam = exam;
            examVM.Questions = new List<Question>();
            examVM.Questions.AddRange(questions);
            return View(examVM);
        }

        //[HttpPost]
        public async Task<ActionResult> SaveAnswers(string examId, List<List<string>> answers)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(examId) || !ObjectId.TryParse(examId, out Id))
            {
                return Json(new { success = false, responseText = "Please select an exam." }, JsonRequestBehavior.AllowGet);
            }
            var result = await Db.Exams.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return Json(new { success = false, responseText = "Exam not found, try again later." }, JsonRequestBehavior.AllowGet);
            }

            if (answers.Any(aa=>aa.Any(b=>String.IsNullOrEmpty(b))))
            {
                return Json(new { success = false, responseText = "Please Fill all the blanks!" }, JsonRequestBehavior.AllowGet);
            }
        

            var questions = Db.Questions.Find(q => q.ExamId == new ObjectId(examId)).ToList();
            if (AlreadyAnswered(User.Identity.GetUserId(), questions))
            {
                return Json(new { success = false, responseText = "You've already answered the questions" }, JsonRequestBehavior.AllowGet);
            }

            if (answers.Count != questions.Count)
            {
                return Json(new { success = false, responseText = "Please fill all the blanks" }, JsonRequestBehavior.AllowGet);
            }
            int i = 0, j = 0;
            var a = answers[i][j];
            //foreach (var u in questions)
            //{
            //    foreach (var b in u.Blanks)
            //    {
            //        if (b.StudentAnswers == null)
            //        {
            //            b.StudentAnswers = new List<StudentAnswer>();
            //        }
            //        b.StudentAnswers.Add(new StudentAnswer
            //        {
            //            StudentId = new ObjectId(User.Identity.GetUserId()),
            //            AnswerTxt = answers[i][j],
            //            Score = 0 //CalculateStudentScore(b,answers[i][j])
            //        });
            //        j++;
            //    }
            //    j = 0;
            //    i++;
            //    Db.Questions.ReplaceOne(t => t.Id.Equals(u.Id), u, new UpdateOptions { IsUpsert = true });

            //}
            questions.ForEach(u =>{
                u.Blanks.ForEach(b =>{
                    if (b.StudentAnswers == null) b.StudentAnswers = new List<StudentAnswer>();
                    b.StudentAnswers.Add(new StudentAnswer
                    {
                        StudentId = new ObjectId(User.Identity.GetUserId()),
                        AnswerTxt = answers[i][j],
                        Score = CalculateStudentScore(b, answers[i][j])
                    });
                    j++;
                });
                j = 0;
                i++;
                Db.Questions.ReplaceOne(t => t.Id.Equals(u.Id), u, new UpdateOptions { IsUpsert = true });
            });

            return Json(new { success = true, responseText = "Answers Saved!" }, JsonRequestBehavior.AllowGet);

        }

        public bool AlreadyAnswered(string userId, List<Question> questions)
        {
            return questions.Any(q => q.Blanks!=null && q.Blanks.Any(b => b.StudentAnswers!=null && b.StudentAnswers.Any(sa => sa.StudentId == new ObjectId(userId))));
        }

        public int CalculateStudentScore(Blank blank, string answer)
        {
            var result = 0;
            //TODO: calculate score

            return result;
        }




        //GET: /Exams/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Exam exam)
        {
            var Id = new ObjectId();
            ObjectId.TryParse(User.Identity.GetUserId(), out Id);
            exam.TeacherId = Id;
            if (ModelState.IsValid)
            {
                Db.Exams.InsertOne(exam);
                return RedirectToAction("Index");
            }

            return View(exam);
        }


        // GET: /Exams/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = await Db.Exams.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: /Exams/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = await Db.Exams.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: /Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id, Exam exam)
        {
            if (!string.IsNullOrEmpty(exam.Course))
            {
                exam.Id = new ObjectId(id);
                await Db.Exams.ReplaceOneAsync(t => t.Id.Equals(exam.Id), exam, new UpdateOptions { IsUpsert = true });
                return RedirectToAction("Index");
            }
            return View(exam);
        }

        // GET: /Exams/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var Id = new ObjectId();
            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = await Db.Exams.Find(t => t.Id == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: /Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await Db.Exams.DeleteOneAsync(t => t.Id == new ObjectId(id));
            return RedirectToAction("Index");
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