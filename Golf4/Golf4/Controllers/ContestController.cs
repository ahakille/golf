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
        //GET: Contests
        public ActionResult Contests()
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            ContestModels model = new ContestModels();
            model.AllContests = contests.GetAllContests();

            return View(model);
        }

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
        [Authorize(Roles = "2")]
        public ActionResult create(ContestModels model)
        {
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);
            }
            
            ContestModels.MakeCompetition create = new ContestModels.MakeCompetition();
            create.Createcontest(Convert.ToInt32(User.Identity.Name) , model.Name, model.Timestart, model.Timeend, model.CloseTime, model.MaxPlayers, model.description);
            return Redirect("index");
        }
        [Authorize(Roles = "2")]
        public ActionResult edit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult edit(ContestModels model)
        {

            return View();
        }
        [Authorize(Roles = "2")]
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

        //Get Member
        [HttpGet]
        public ActionResult Member()
        {
            ContestModels model = new ContestModels();
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            ContestModels.Contest contests = new ContestModels.Contest();
            MemberModels members = new MemberModels();
            model.ContestMembers = contests.MembersInContest(model.ContestID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Member(ContestModels model)
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            contests.AddPlayersToContest(model.ContestID, Convert.ToInt32(User.Identity.Name));
            return Redirect("Member");
        }

        [Authorize(Roles = "2")]
        public ActionResult Addplayers()
        {
            ContestModels model = new ContestModels();
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            int user_id = Convert.ToInt16(Request.QueryString["member"]);
            ContestModels.Contest contests = new ContestModels.Contest();
            contests.AddPlayersToContest(model.ContestID, user_id);
            return RedirectToAction("admin", "contest", new { cont = model.ContestID });
        }

        public ActionResult RemovePlayers()
        {
            ContestModels model = new ContestModels();
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            int user_id = Convert.ToInt16(Request.QueryString["member"]);
            ContestModels.Contest contests = new ContestModels.Contest();
            contests.DeletePlayersFromContest(model.ContestID, user_id);
            return RedirectToAction("admin", "contest", new { cont = model.ContestID });
        }

        public ActionResult AddPlayersAsMember()
        {
            ContestModels model = new ContestModels();
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            int user_id = Convert.ToInt16(User.Identity.Name);
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
        public ActionResult deletemember()
        {
            ContestModels model = new ContestModels();
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            ContestModels.Contest contests = new ContestModels.Contest();
            contests.DeletePlayersFromContest(model.ContestID, Convert.ToInt32(User.Identity.Name));
            return RedirectToAction("index", "Member");
        }
        public ActionResult CollectResult()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult CollectResult(ContestScore model)
        {
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);
            }


            return null;
        }
    
    }

}