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
    public class ContestController : Controller
    {
        // GET: Contest
        public ActionResult Index()
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            ContestModels model = new ContestModels();
            model.AllContests = contests.GetAllContests();

            return View(model);
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(ContestModels model)
        {
            ContestModels.MakeCompetition create = new ContestModels.MakeCompetition();
            create.Createcontest(1, model.Name, model.Timestart, model.Timeend, model.CloseTime, model.MaxPlayers, model.description);
            return Redirect("index");
        }

        public ActionResult Admin()
        {
            ContestModels model = new ContestModels();
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            ContestModels.Contest contests = new ContestModels.Contest();
            MemberModels members = new MemberModels();
            
            model.ContestMembers = contests.MembersInContest(model.ContestID);
            model.AllContests = members.CollectAllMembers();
           
            return View(model);
        }
        public ActionResult Addplayers()
        {
            ContestModels model = new ContestModels();
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            int user_id = Convert.ToInt16(Request.QueryString["member"]);
            ContestModels.Contest contests = new ContestModels.Contest();
            contests.AddPlayersToContest(model.ContestID, user_id);
            return Redirect("admin");
        }

        public ActionResult Competition()
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            ContestModels Model = new ContestModels();
            Model.AllContests = contests.GetAllContests();

            return View(Model);
        }
    }

}