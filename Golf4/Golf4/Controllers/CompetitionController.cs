using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf4.Models;

namespace Golf4.Controllers
{
    public class CompetitionController : Controller
    {
        // GET: Competition
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(CompetitionModels model)
        {
            return View();
        }
    }

}