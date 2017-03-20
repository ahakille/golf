using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Golf4.Models;
using Npgsql;

namespace Golf4.Controllers
{
    public class HomeCompController : Controller
    {
        [AllowAnonymous]
        // GET: HomeComp
        public ActionResult Index()
        {
            HomeCompModels.Contest contests = new HomeCompModels.Contest();
            HomeCompModels Model = new HomeCompModels();
            Model.AllContests = contests.GetAllContests();

            return View(Model);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult CompetitionInfo()
        {
            HomeCompModels.Contest contests = new HomeCompModels.Contest();
            HomeCompModels hcm = new HomeCompModels();
            hcm.CompetitionInfo = contests.Info4Competition(hcm.ContestID);

            return View(hcm);
        }

    }
        
}