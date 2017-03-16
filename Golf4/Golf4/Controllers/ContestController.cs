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

        public ActionResult Admin(int contestid)
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            DataTable members = contests.MembersInContest(contestid);
            return View(members);
        }

        public ActionResult GetAllMembers()
        {
            MemberModels member = new MemberModels();
            return View(member.CollectAllMembers());
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