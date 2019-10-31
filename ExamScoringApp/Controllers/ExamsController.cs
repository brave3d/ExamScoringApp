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
            //var examVms = new List<Exam>();
            //foreach (var item in exams)
            //{
            //    examVms.Add(new TextVM
            //    {
            //        Id = item.Id,
            //        IsTagged = await Tagged(item),
            //        ReadOnly = item.ReadOnly,
            //        Source = item.Source,
            //        Txt = item.Txt
            //    });
            //}

            return View(exams);
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


        //GET: /Exams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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