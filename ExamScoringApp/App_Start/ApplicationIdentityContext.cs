namespace ExamScoringApp
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using AspNet.Identity.MongoDB;
	using Models;
	using MongoDB.Driver;
    using ExamScoringApp.Models.Base;
    using System.Configuration;

    public class ApplicationIdentityContext : IDisposable
	{
		public static ApplicationIdentityContext Create()
		{
			var client = new MongoClient(ConfigurationManager.ConnectionStrings["ContactListAppDefaultConnection"].ToString());
			var database = client.GetDatabase(ConfigurationManager.AppSettings["DbName"]);
			var users = database.GetCollection<ApplicationUser>("users");
			var roles = database.GetCollection<IdentityRole>("roles");

            var questions = database.GetCollection<Question>("Questions");
            var exams = database.GetCollection<Exam>("Exams");

            return new ApplicationIdentityContext(users, roles, questions, exams);
		}

		private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users,
            IMongoCollection<IdentityRole> roles,
            IMongoCollection<Question> questions,
            IMongoCollection<Exam> exams)
        {
			Users = users;
			Roles = roles;
            Questions = questions;
            Exams = exams;

        }

		public IMongoCollection<IdentityRole> Roles { get; set; }
		public IMongoCollection<ApplicationUser> Users { get; set; }
		public IMongoCollection<Question> Questions { get; set; }
        public IMongoCollection<Exam> Exams { get; set; }


        public Task<List<IdentityRole>> AllRolesAsync()
		{
			return Roles.Find(r => true).ToListAsync();
		}

		public void Dispose()
		{
		}
	}
}