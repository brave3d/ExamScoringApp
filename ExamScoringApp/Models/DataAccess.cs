using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;

namespace ExamScoringApp.Models
{
    public class DataAccess
    {
        MongoClient _client;
        MongoServer _server;
        public IMongoDatabase _db;
        public DataAccess()
        {
            _client = new MongoClient(ConfigurationManager.ConnectionStrings["ContactListAppDefaultConnection"].ToString());
             _db = _client.GetDatabase(ConfigurationManager.AppSettings["DbName"]);
        }

        //public object GetExams()
        //{
        //    //var collection = _db.GetCollection<Sentence>("Sentences");
        //    List<Exam> list ;

        //    var collection3 = _db.GetCollection<BsonDocument>("Sentences");
        //    var filter = Builders<BsonDocument>.Filter.Eq("Text_ID", "5bc0bd2b92166436d4f00ccf");
        //    var result = collection3.Find(filter).Skip(0).Limit(10).ToList().ToJson();
        //    //try
        //    //{
        //    //    list = collection.Find(_ => true).ToList();

        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    int a = 10;
        //    //   throw;
        //    //}
        //    return result;
        //}

        public IEnumerable<Exam> GetTxts()
        {
            List<Exam> list;
            var collection3 = _db.GetCollection<Exam>("Exams");
            var filter = Builders<Exam>.Filter.Empty;
            var result = collection3.Find(filter).Skip(0).Limit(10).ToList();
            return result;
        }

        //public Book GetBook(ObjectId id)
        //{
        //    var res = Query<Book>.EQ(p => p.Id, id);
        //    return _db.GetCollection<Book>("Books").FindOne(res);
        //}
        //public Book Create(Book p)
        //{
        //    _db.GetCollection<Book>("Books").Save(p);
        //    return p;
        //}
        //public void Update(ObjectId id, Book p)
        //{
        //    p.Id = id;
        //    var res = Query<Book>.EQ(pd => pd.Id, id);
        //    var operation = Update<Book>.Replace(p);
        //    _db.GetCollection<Book>("Books").Update(res, operation);
        //}
        //public void Remove(ObjectId id)
        //{
        //    var res = Query<Book>.EQ(e => e.Id, id);
        //    var operation = _db.GetCollection<Book>("Books").Remove(res);
        //}
    }
}