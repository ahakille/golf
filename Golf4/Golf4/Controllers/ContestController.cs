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
            model.AllContests = contests.GetAllContestsGuests();

            return View(model);
        }

        // GET: Contest
        public ActionResult Index()
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            ContestModels model = new ContestModels();
            model.AllContests = contests.GetAllContests();

            model.PublishedContests = contests.GetPublishedContests();

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
        [HttpPost]
        public ActionResult Admin(ContestModels model)
        {
            ContestModels.Contest runforestrun = new ContestModels.Contest();
            runforestrun.MembersInContestTimeSetting(model.ContestID);
            return RedirectToAction("Admin", "contest", new { cont = model.ContestID });
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

            DataTable dt = contests.GetNameAndDate(model.ContestID);
            foreach (DataRow dr in dt.Rows)
            {
                model.Name = (string)dr["cn"];
                model.Timestart = (DateTime)dr["timestart"];
            }

            model.NameAndDate = model.Name + ": " + model.Timestart.ToShortDateString(); 
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
            DataTable dt = contests.GetNameAndDate(model.ContestID);
            foreach (DataRow dr in dt.Rows)
            {
                model.Name = (string)dr["cn"];
                model.Timestart = (DateTime)dr["timestart"];
            }

            model.NameAndDate = model.Name + ": " + model.Timestart.ToShortDateString();

            return View(model);
        }

        [HttpPost]
        public ActionResult Member(ContestModels model)
        {
            ContestModels.Contest contests = new ContestModels.Contest();
            contests.AddPlayersToContest(model.ContestID, Convert.ToInt32(User.Identity.Name));
            return RedirectToAction("Member", "contest", new { cont = model.ContestID });
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
            ContestScore model = new ContestScore();
            model.User_id = Convert.ToInt16(Request.QueryString["member"]);
            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);

            MemberModels member = new MemberModels();
            DataTable dt = member.CollectOneMember(model.User_id);
            foreach (DataRow dr in dt.Rows)
            {

                model.Name = (string)dr["firstname"];
                model.Name += " ";
                model.Name += (string)dr["lastname"];
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CollectResult(ContestScore model)
        {
            int[] whacks = { model.Hole1, model.Hole2, model.Hole3, model.Hole4, model.Hole5, model.Hole6, model.Hole7,
                model.Hole8, model.Hole9, model.Hole10, model.Hole11, model.Hole12, model.Hole13, model.Hole14,
                model.Hole15, model.Hole16, model.Hole17, model.Hole18 };

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fyll i korrekt, Endast slag");
                // om inte rätt format
                return View(model);
            }
            ContestScore contest = new ContestScore();
            PostgresModels Database3 = new PostgresModels();
            DataTable last = Database3.SqlQuery("SELECT SUM(par) ::integer FROM holes WHERE id BETWEEN 1 AND 18;", PostgresModels.list = new List<NpgsqlParameter>());
            foreach (DataRow dr2 in last.Rows)
            {
                contest.Par = (int)dr2["sum"];
            }

            PostgresModels Database4 = new PostgresModels();
            DataTable member = Database4.SqlQuery("SELECT id, firstname, lastname, hcp, golfid, gender FROM members where id = @id", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", model.User_id)
            });
            foreach (DataRow dr3 in member.Rows)
            {
                contest.Firstname = (string)dr3["firstname"];
                contest.Lastname = (string)dr3["lastname"];
                contest.HCP = (double)dr3["hcp"];
                contest.GolfID = (string)dr3["golfid"];
                contest.Gender = (int)dr3["gender"];
            }

            int teeid = 1;
            if (contest.Gender == 1)
            {
                teeid = 3;
            }
            
            PostgresModels Database6 = new PostgresModels();
            DataTable tees = Database6.SqlQuery("SELECT * FROM tees where id = @teeid", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@teeid", teeid)
            });
            foreach (DataRow dr5 in tees.Rows)
            {
                contest.TeeID = (int)dr5["id"];
                contest.WomanCR = (double)dr5["woman_cr"];
                contest.WomanSlope = (int)dr5["woman_slope"];
                contest.ManCR = (double)dr5["man_cr"];
                contest.ManSlope = (int)dr5["man_slope"];
            }

            if (contest.Gender == 1)
            {
                double ASM = contest.HCP * (contest.ManSlope / 113) + (contest.ManCR - contest.Par);
                double RASM = Math.Round(ASM, MidpointRounding.AwayFromZero);
                int strokes = Convert.ToInt32(RASM);
                contest.Strokes = strokes;
                int holes = 18;
                contest.Counting = (strokes / holes);
                strokes %= holes;
                contest.Rest = strokes;
            }

            else
            {
                double ASW = contest.HCP * (contest.WomanSlope / 113) + (contest.WomanCR - contest.Par);
                double RASW = Math.Round(ASW, MidpointRounding.AwayFromZero);
                int strokes = Convert.ToInt32(RASW);
                contest.Strokes = strokes;
                int holes = 18;
                contest.Counting = (strokes / holes);
                strokes %= holes;
                contest.Rest = strokes;
            }

            PostgresModels Database7 = new PostgresModels();
            DataTable contesthole = Database7.SqlQuery("SELECT * FROM holes", PostgresModels.list = new List<NpgsqlParameter>());
            int i = 0;
            
            foreach (DataRow dr6 in contesthole.Rows)
            {
                ContestScore hole = new ContestScore();
                hole.Hole = (int)dr6["id"];
                hole.HoleHCP = (int)dr6["hcp"];
                hole.HolePar = (int)dr6["par"];

                int strokes = (hole.HCP <= contest.Rest && contest.Rest != 0) ? (contest.Counting + 1) : contest.Counting;
                int calc = hole.HolePar + strokes;
                int better = calc - whacks[i];
                
                if (calc + 1 == whacks[i])
                {
                    model.Result += 1;
                }
                if (calc == whacks[i])
                {
                    model.Result += 2;
                }
                else if (calc > whacks[i])
                {
                    model.Result += (2 + better);
                }
                i++;
            }
            PostgresModels Database8 = new PostgresModels();
            Database8.SqlNonQuery("UPDATE players SET result = @result WHERE memberid = @user AND contestid = @contest", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@result", model.Result),
                new NpgsqlParameter("@contest", model.ContestID),
                new NpgsqlParameter("@user", model.User_id)
            }
                );

            return RedirectToAction("admin", "contest", new { cont = model.ContestID });
        }
    
        public ActionResult ResultList()
        {
            ContestModels.Result results = new ContestModels.Result();
            ContestModels model = new ContestModels();
            ContestModels.Contest contest = new ContestModels.Contest();

            model.ContestID = Convert.ToInt16(Request.QueryString["cont"]);
            //Hårdkodat
            model.ViewResultList = results.GetResultList(model.ContestID);

            DataTable dt = contest.GetNameAndDate(model.ContestID);
            foreach (DataRow dr in dt.Rows)
            {
                model.Name = (string)dr["cn"];
                model.Timestart = (DateTime)dr["timestart"];
            }

            model.NameAndDate = model.Name + ": " + model.Timestart.ToShortDateString();
            return View(model);
            return View(model);
        }
    }

}