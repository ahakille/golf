using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Golf4.Models;

namespace Golf4.Controllers
{
    public class ContestController : Controller
    {
        // GET: Competition
        public ActionResult Index()
        {
            //DataTable dt = Models.CompetitionModels.Get
            return View();
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(ContestModels model)
        {
            return View();
        }
    }

}