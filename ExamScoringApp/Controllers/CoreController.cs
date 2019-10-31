using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamScoringApp.Models;
using Microsoft.AspNet.Identity.Owin;

namespace ExamScoringApp.Controllers
{
    [Authorize]
    public class CoreController : Controller
    {
        public ApplicationIdentityContext Db => HttpContext.GetOwinContext().GetUserManager<ApplicationIdentityContext>();


        public ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }



}