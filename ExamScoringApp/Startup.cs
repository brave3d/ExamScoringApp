using Microsoft.Owin;
using Owin;
using System.Web.ModelBinding;
using ExamScoringApp.Models;

//[assembly: OwinStartupAttribute(typeof(ExamScoringApp.Startup))]
namespace ExamScoringApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
       

        }
    }
}
