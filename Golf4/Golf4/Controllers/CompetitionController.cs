using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf4.Controllers
{
    public class CompetitionController : Controller
    {
        // GET: Competition
        public ActionResult Index()
        {
            return View();
        }
    }
}