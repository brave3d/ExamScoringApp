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
    [Authorize(Roles = "Admin")]
    public class StudentsPointsController : CoreController
    {

        // GET: /StudentsPoints/

        public async Task<ActionResult> Index()
        {
            return View();
        }

        //public async Task<ActionResult> Index()
        //{
        //    var sentences = await Db.Sentences.Find(s => true).ToListAsync();
        //    var groupedSentences = sentences.GroupBy(s => s.Text_ID).ToList();
        //    var studentsPoints = new List<StudentsPointsVM>();
        //    var studentsTextPoints = new List<StudentsTextPointsVM>();

        //    foreach (var gs in groupedSentences)
        //    {
        //        foreach (var sentence in gs)
        //        {
        //            if (TokenizedByAdmin(sentence))
        //            {
        //                var adminToken = FindAdminTokens(sentence);
        //                var adminTaggedTokens = adminToken.TaggedTokens;
        //                foreach (var token in sentence.Tokens.Except(new List<Token>() { adminToken }))
        //                {
        //                    var student = Db.Users.Find(u => !u.Roles.Contains("Admin") && u.Roles.Contains("Student") && u.Id == token.UserId.ToString()).FirstOrDefault();
        //                    if (student == null)
        //                        continue;
        //                    var studentTaggedTokens = token.TaggedTokens;
        //                    var sp = new StudentsPointsVM();
        //                    sp.StudentNo = student?.StudentNo;
        //                    sp.StudentId = student?.Id;
        //                    sp.StudentName = student?.StudentName;
        //                    sp.CorrectTokens = String.Join(", ", adminTaggedTokens.Select(t => t.Token).ToList());
        //                    sp.AnsweredTokens = String.Join(", ", studentTaggedTokens.Select(t => t.Token).ToList());
        //                    sp.Points = Math.Round(CalculatePoints(adminTaggedTokens, studentTaggedTokens), 3);
        //                    sp.SentenceId = sentence.Id;
        //                    sp.TextId = gs.Key;
        //                    studentsPoints.Add(sp);
        //                }
        //            }
        //        }
        //    }


        //    var groupedByStudentNos = studentsPoints.GroupBy(sp => sp.StudentId).ToList();
        //    foreach (var groupedByStudentNo in groupedByStudentNos)
        //    {
        //        var byText = groupedByStudentNo.GroupBy(gsn => gsn.TextId).ToList();
        //        foreach (var item in byText)
        //        {
        //            var stp = new StudentsTextPointsVM();

        //            stp.StudentId = groupedByStudentNo.Key;
        //            stp.StudentNo = groupedByStudentNo.FirstOrDefault().StudentNo;
        //            stp.StudentName = groupedByStudentNo.FirstOrDefault().StudentName;
        //            stp.TextId = item.Key;
        //            stp.Average = CalculatePointsOfText(item.Key,item, stp.StudentNo);
        //            stp.Text = Db.Texts.Find(t=>t.Id==item.Key).FirstOrDefault().Txt;
        //            studentsTextPoints.Add(stp);
        //        }

        //    }
        
        //    return View(studentsTextPoints);
        //}

    
        //public async Task<ActionResult> StudentsSentencesPoints(string id,string studentId)
        //{
        //    var textSentences = await Db.Sentences.Find(s => s.Text_ID== new ObjectId(id)).ToListAsync();
        //    List<Sentence> sentences = new List<Sentence>();
        //    foreach (var item in textSentences)
        //    {
        //        if (item.Tokens!=null && item.Tokens.Count>0)
        //        {
        //            sentences.Add(item);
        //        }
        //    }
        //    sentences = sentences.Where(s => s.Tokens.Any(t => t.UserId == new ObjectId(studentId))).ToList();
        //    var studentsPoints = new List<StudentsPointsVM>();
        //    foreach (var sentence in sentences)
        //    {
        //        if (TokenizedByAdmin(sentence))
        //        {
        //            var adminToken = FindAdminTokens(sentence);
        //            var adminTaggedTokens = adminToken.TaggedTokens;
        //            var sentenceTokens = sentence.Tokens?.Where(t => t.UserId == new ObjectId(studentId));
        //            if (sentenceTokens==null )
        //            {
        //                continue;
        //            }
        //            foreach (var token in sentenceTokens)
        //            {
        //                var student = Db.Users.Find(u => u.Id == studentId).FirstOrDefault();
        //                if (student == null)
        //                    continue;
        //                var studentTaggedTokens = token.TaggedTokens;
        //                var sp = new StudentsPointsVM();
        //                sp.StudentNo = student?.StudentNo;
        //                sp.StudentName = student?.StudentName;
        //                sp.CorrectTokens = String.Join(", ", adminTaggedTokens.Select(t => t.Token).ToList());
        //                sp.AnsweredTokens = String.Join(", ", studentTaggedTokens.Select(t => t.Token).ToList());
        //                sp.Points = Math.Round(CalculatePoints(adminTaggedTokens, studentTaggedTokens), 3);
        //                studentsPoints.Add(sp);
        //            }
        //        }

        //    }
        //    return View(studentsPoints);
        //}
        //private double CalculatePointsOfText(ObjectId textId,IGrouping<ObjectId,StudentsPointsVM> studentsPointsByText, string studentNo)
        //{
        //    var textSentences = Db.Sentences.Find(s => s.Text_ID == textId).ToList();
        //    var taggedSentencesPoints = 0.0;

        //    foreach (var item in studentsPointsByText)
        //    {
        //        taggedSentencesPoints += item.Points;
        //    }
        //    if (studentsPointsByText!=null && studentsPointsByText.ToList().Count>0)
        //    {
        //        return taggedSentencesPoints / textSentences.Count;
        //    }
        //    return 0.0;
        //}

        private double CalculatePoints(List<TaggedToken> correctToken, List<TaggedToken> answeredToken)
        {
            var studentCorrectTokens = 0.0;
            for (int i = 0; i < answeredToken.Count; i++)
            {
                if (correctToken.Any(ct=>ct.Token== answeredToken[i].Token) /*&& correctToken.IndexOf(answeredToken[i])==i*/)
                    studentCorrectTokens++;

            }
            return (studentCorrectTokens / correctToken.Count)*100 ;
        }

     


        //private bool TokenizedByAdmin(Sentence sentence)
        //{
        //    if (sentence.Tokens!=null && sentence.Tokens.Any())
        //        foreach (var token in sentence.Tokens)
        //        {
        //            var tokenString = token.UserId.ToString();
        //            var admin = Db.Users.Find(u => u.Id == tokenString && u.Roles.Contains("Admin")).FirstOrDefault();
        //            if (admin != null)
        //                return true;
        //        }
        //    return false;
        //}

        //private Token FindAdminTokens(Sentence sentence)
        //{
        //    foreach (var token in sentence.Tokens)
        //    {
        //        var admin = Db.Users.Find(u => u.Roles.Contains("Admin") && u.Id == token.UserId.ToString()).FirstOrDefault();
        //        if (admin != null)
        //            return token;
        //    }
        //    return null;
        //}
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