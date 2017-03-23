using Golf4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf4.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        
        public ActionResult Index()

        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Contests()
        {
            return View();
        }

        public ActionResult Results()
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            ContestModels model = new ContestModels();

            model.PublishedContests = contests.GetPublishedContests();

            return View(model);
        }
    }
}