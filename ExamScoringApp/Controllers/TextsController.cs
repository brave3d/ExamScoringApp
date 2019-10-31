//using Microsoft.AspNet.Identity;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using MongoDB.Driver.Builders;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using ExamScoringApp.Models;

//namespace ExamScoringApp.Controllers
//{
//    [Authorize]
//    public class TextsController : CoreController
//    {
     
//        public JsonResult GetSentences()
//        {
//            var mdb = new DataAccess();
//            var sentences = mdb.GetTexts();
//            return Json(sentences, JsonRequestBehavior.AllowGet);
//        }



//        // GET: /Texts/
//        public async Task<ActionResult> Index()
//        {
//            var texts = await Db.Texts.Find(s => true).ToListAsync();
//            var textVms = new List<TextVM>();
//            foreach (var item in texts)
//            {
//                textVms.Add(new TextVM {
//                    Id = item.Id,
//                    IsTagged = await Tagged(item),
//                    ReadOnly = item.ReadOnly,
//                    Source = item.Source,
//                    Txt = item.Txt
//                });
//            }

//            return View(textVms);
//        }

//        private async Task<bool> Tagged(Text text)
//        {
//            var result = false;
//            var sentences = await Db.Sentences.Find(t => t.Text_ID == text.Id).ToListAsync();
//            if (sentences.All(s=> TaggedByUser(s)))
//            {
//                result = true;
//            }
            
//            return result;
//        }



//        private bool TaggedByUser(Sentence sentence)
//        {
//            var currentUserId = HttpContext.GetOwinContext().Request.User.Identity.GetUserId();
//            if (sentence.Tokens != null && sentence.Tokens.Any())
//            {
//                foreach (var token in sentence.Tokens)
//                {
//                    if (token.UserId.ToString() == currentUserId)
//                        return true;
//                }
//            }
//            return false;
//        }




//        // GET: /Texts/Details/5
//        public async Task<ActionResult> Details(string id)
//        {
//            var Id=new ObjectId();
//            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id,out Id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            var result =  await Db.Texts.Find(t => t.Id == Id).FirstOrDefaultAsync();
//            if (result == null)
//            {
//                return HttpNotFound();
//            }
//            return View(result);
//        }



//        private bool TaggedByAdmin(Sentence sentence)
//        {
//            if (sentence.Tokens != null && sentence.Tokens.Any())
//            {
//                foreach (var token in sentence.Tokens)
//                {
//                    var tokenUserId = token.UserId.ToString();
//                    var admin = Db.Users.Find(u => u.Id == tokenUserId && u.Roles.Contains("Admin")).FirstOrDefault();
//                    if (admin != null)
//                        return true;
//                }
//            }
//            return false;
//        }

//        // GET: /Texts/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: /Texts/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(Text text)
//        {
//            if (ModelState.IsValid)
//            {
//                Db.Texts.InsertOne(text);
//                return RedirectToAction("Index");
//            }

//            return View(text);
//        }

//        // GET: /Texts/Edit/5
//        public async Task<ActionResult> Edit(string id)
//        {
//            var Id = new ObjectId();
//            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var result = await Db.Texts.Find(t => t.Id == Id).FirstOrDefaultAsync();
//            if (result == null)
//            {
//                return HttpNotFound();
//            }
//            return View(result);
//        }

//        // POST: /Texts/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit(string id,Text text)
//        {
//            if (!string.IsNullOrEmpty(text.Txt))
//            {
//                text.Id= new ObjectId(id);
//                await Db.Texts.ReplaceOneAsync(t => t.Id.Equals(text.Id), text, new UpdateOptions { IsUpsert = true });
//                return RedirectToAction("Index");
//            }
//            return View(text);
//        }

//        // GET: /Texts/Delete/5
//        public async Task<ActionResult> Delete(string id)
//        {
//            var Id = new ObjectId();
//            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            var result = await Db.Texts.Find(t => t.Id == Id).FirstOrDefaultAsync();
//            if (result == null)
//            {
//                return HttpNotFound();
//            }
//            return View(result);
//        }

//        // POST: /Texts/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(string id)
//        {
//            await Db.Texts.DeleteOneAsync(t => t.Id == new ObjectId(id));
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                Db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//    }
//}