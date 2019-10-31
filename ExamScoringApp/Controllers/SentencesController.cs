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
//    public class SentencesController : CoreController
//    {
    

//        // GET: /Sentences/
//        public async Task<ActionResult> Index()
//        {
//            var sentences = await Db.Sentences.Find(s => true).ToListAsync();
//            return View(sentences);
//        }

//        // GET: /RemoveAll/
//        [Authorize(Roles ="Admin")]

//        public async Task<ActionResult> RemoveAll()
//        {
//            await Db.Sentences.DeleteManyAsync(s => true);
//            return RedirectToAction("Index");
//        }


//        public async Task<ActionResult> SentencesList(string id)
//        {
//            var Id = new ObjectId();
//            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            //var result = await Db.Texts.Find(t => t.Id == Id).FirstOrDefaultAsync();
//            var result = await Db.Sentences.Find(t => t.Text_ID == Id && !t.IsTagged).ToListAsync();

//            if (result == null)
//            {
//                return HttpNotFound();
//            }

//            var sentences = new List<Sentence>();
//            char[] delimiters = new char[] {'.'};
//            //result.Txt = Regex.Replace(result.Txt.Trim(), @"\s+", " ");
//            //var splits = result.Txt.Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();

//            //splits.ForEach(s => sentences.Add(new Sentence()
//            //{
//            //    Text = s +" .",
//            //    Text_ID = result.Id
//            //}));

//            //if (Db.Sentences.CountDocuments(s=>s.Text_ID == result.Id) > 0)
//            //{
//            //    sentences = await Db.Sentences.Find(s => s.Text_ID == result.Id).ToListAsync();
//            //}
//            //else
//            //{
//            //    await Db.Sentences.InsertManyAsync(sentences);
//            //    sentences = await Db.Sentences.Find(s => s.Text_ID == result.Id).ToListAsync();
//            //}
//            sentences.AddRange(result.ToList());
//            var sentencesList = new List<SentenceVM>();
//            sentences.ForEach(s => sentencesList.Add(new SentenceVM
//            {
//                Id= s.Id,
//                Text = s.Text,
//                Text_ID = s.Text_ID,
//                TaggedByAdmin = TaggedByAdmin(s),
//                TaggedByMe= TaggedByMe(s)
//            }));
          
//            return View(sentencesList);

//        }


//        //public async Task<ActionResult> TaggedSentences(string id)
//        //{
//        //    var Id = new ObjectId();
//        //    if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }

//        //    //var result = await Db.Texts.Find(t => t.Id == Id).FirstOrDefaultAsync();
//        //    var result = await Db.Sentences.Find(t => t.Text_ID == Id && t.IsTagged).ToListAsync();

//        //    if (result == null)
//        //    {
//        //        return HttpNotFound();
//        //    }

//        //    var sentences = new List<Sentence>();
//        //    char[] delimiters = new char[] { '.' };
          
//        //    sentences.AddRange(result.ToList());
//        //    var sentencesList = new List<SentenceVM>();
//        //    sentences.ForEach(s => sentencesList.Add(new SentenceVM
//        //    {
//        //        Id = s.Id,
//        //        Text = s.Text,
//        //        Text_ID = s.Text_ID,
//        //        TaggedByAdmin = TaggedByAdmin(s),
//        //        TaggedByMe = TaggedByMe(s)
//        //    }));
//        //    return View(sentencesList);

//        //}

//        //private bool TaggedByMe(Sentence sentence)
//        //{
//        //    var currentUserId = HttpContext.GetOwinContext().Request.User.Identity.GetUserId();
//        //    if (sentence.Tokens != null && sentence.Tokens.Any())
//        //    {
//        //        foreach (var token in sentence.Tokens)
//        //        {
//        //            if (token.UserId.ToString() == currentUserId)
//        //                return true;
//        //        }
//        //    }
//        //    return false;
//        //}

//        //private bool TaggedByAdmin(Sentence sentence)
//        //{
//        //    if (sentence.Tokens != null && sentence.Tokens.Any())
//        //    {
//        //        foreach (var token in sentence.Tokens)
//        //        {
//        //            var tokenUserId = token.UserId.ToString();
//        //            var admin = Db.Users.Find(u => u.Id == tokenUserId && u.Roles.Contains("Admin")).FirstOrDefault();
//        //            if (admin != null)
//        //                return true;
//        //        }
//        //    }
//        //    return false;
//        //}

//        //private Token FindMyTokens(Sentence sentence)
//        //{
//        //    if (sentence != null && sentence.Tokens != null)
//        //    {
//        //        foreach (var token in sentence.Tokens)
//        //        {
//        //            if (token.UserId.ToString() == HttpContext.GetOwinContext().Request.User.Identity.GetUserId())
//        //                return token;
//        //        }
//        //    }
//        //    return null;
//        //}

//        //private Token FindAdminTokens(Sentence sentence)
//        //{

//        //    if (sentence!=null && sentence.Tokens!=null)
//        //    {
//        //        foreach (var token in sentence.Tokens)
//        //        {
//        //            var admin = Db.Users.Find(u => u.Roles.Contains("Admin") && u.Id == token.UserId.ToString()).FirstOrDefault();
//        //            if (admin != null)
//        //                return token;
//        //        }
//        //    }
//        //    return null;
//        //}

//        //public async Task<ActionResult> TagSentence(string id)
//        //{
//        //    var Id = new ObjectId();
//        //    if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    var result = await Db.Sentences.Find(t => t.Id == Id).FirstOrDefaultAsync();
//        //    var text = await Db.Texts.Find(t => t.Id == result.Text_ID).FirstOrDefaultAsync();
//        //    ViewBag.readOnly = text.ReadOnly;
//        //    ViewBag.tokenizedByAdmin = TaggedByAdmin(result);

//        //    if (result == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(result);
//        //}


//        //public async Task<ActionResult> EditTag(string id)
//        //{
//        //    var Id = new ObjectId();
//        //    if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    var result = await Db.Sentences.Find(t => t.Id == Id).FirstOrDefaultAsync();
//        //    var text = await Db.Texts.Find(t => t.Id == result.Text_ID).FirstOrDefaultAsync();
//        //    ViewBag.readOnly = text.ReadOnly;
//        //    ViewBag.tokenizedByAdmin = TaggedByAdmin(result);

//        //    if (result == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(result);
//        //}

        



//        //public async Task<ActionResult> AdminTags(string id)
//        //{
//        //    var Id = new ObjectId();
//        //    if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    var result = await Db.Sentences.Find(t => t.Id == Id).FirstOrDefaultAsync();
//        //    var text = await Db.Texts.Find(t => t.Id == result.Text_ID).FirstOrDefaultAsync();
//        //    ViewBag.readOnly = text.ReadOnly;
//        //    ViewBag.tokenizedByAdmin = TaggedByAdmin(result);
//        //    ViewBag.AdminTokens = FindAdminTokens(result)??null;
//        //    if (result == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(result);
//        //}

//        //public async Task<ActionResult> MyTags(string id)
//        //{
//        //    var Id = new ObjectId();
//        //    if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out Id))
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    var result = await Db.Sentences.Find(t => t.Id == Id).FirstOrDefaultAsync();
//        //    var text = await Db.Texts.Find(t => t.Id == result.Text_ID).FirstOrDefaultAsync();
//        //    ViewBag.readOnly = text.ReadOnly;
//        //    ViewBag.myTokens = FindMyTokens(result) ?? null;
//        //    if (result == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(result);
//        //}


//        //[HttpPost]
//        //public async Task<ActionResult> SaveTags(string sentenceId,string userId, Token token)
//        //{
//        //    var Id = new ObjectId();
//        //    if (string.IsNullOrEmpty(sentenceId) || !ObjectId.TryParse(sentenceId, out Id))
//        //    {
//        //        return Json(new { success = false, responseText = "Sentence not found, try again later." }, JsonRequestBehavior.AllowGet);
//        //    }
//        //    var result = await Db.Sentences.Find(t => t.Id == Id).FirstOrDefaultAsync();
//        //    if (result == null)
//        //    {
//        //        return Json(new { success = false, responseText = "Sentence not found, try again later." }, JsonRequestBehavior.AllowGet);
//        //    }

//        //    token.UserId = new ObjectId(userId);
//        //    if (result.Tokens == null)
//        //        result.Tokens = new List<Token>{token};
//        //    else
//        //    {
//        //        if (result.Tokens.Any(t=>t.UserId == token.UserId))
//        //        {
//        //            var userTokenIndex = result.Tokens.FindIndex(t => t.UserId == token.UserId);
//        //            result.Tokens[userTokenIndex] = token;
//        //        }
//        //        else
//        //        result.Tokens.Add(token);

//        //    }
//        //    result.IsTagged = true;
//        //    await Db.Sentences.ReplaceOneAsync(t => t.Id.Equals(Id), result, new UpdateOptions { IsUpsert = true });
//        //    return Json(new { success = true, responseText = "Tags Saved!" }, JsonRequestBehavior.AllowGet);

//        //}


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