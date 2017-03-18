using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Golf4.Models;

namespace Golf4.Controllers
{
    public class ResultController : Controller
    {
        // GET: Result
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResultList()
        {
            ResultModels.Result results = new ResultModels.Result();
            ResultModels model = new ResultModels();
            ContestModels contest = new ContestModels();
            contest.ContestID = Convert.ToInt16(Request.QueryString["cont"]);

            model.ViewResultList = results.GetResultList(1);

            return View(model);
        }
    }
}