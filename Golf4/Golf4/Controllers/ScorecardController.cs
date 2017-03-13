using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf4.Models;
using System.Data;
using Npgsql;

namespace Golf4.Controllers
{
    public class ScorecardController : Controller
    {
        // GET: Scorecard
        public ActionResult ScorecardEmpty()
        {
            ScorecardModel Scorecard = new ScorecardModel();

            PostgresModels Database = new PostgresModels();
            DataTable allData = Database.SqlQuery("SELECT holes.id, tees.name, meters.meters, holes.par, holes.hcp FROM public.holes, public.meters, public.tees WHERE holes.id = meters.hole_id AND meters.tee_id = tees.id;", PostgresModels.list = new List<NpgsqlParameter>());
            Scorecard.AllData = allData;

            PostgresModels Database2 = new PostgresModels();
            DataTable first = Database2.SqlQuery("SELECT SUM(par) ::integer FROM holes WHERE id BETWEEN 1 AND 9;", PostgresModels.list = new List<NpgsqlParameter>());
            foreach (DataRow dr in first.Rows)
            {
                Scorecard.FirstHalfPar = (int)dr["sum"];
            }

            PostgresModels Database3 = new PostgresModels();
            DataTable last = Database3.SqlQuery("SELECT SUM(par) ::integer FROM holes WHERE id BETWEEN 10 AND 18;", PostgresModels.list = new List<NpgsqlParameter>());
            foreach (DataRow dr2 in last.Rows)
            {
                Scorecard.LastHalfPar = (int)dr2["sum"];
            }

            Scorecard.TotalPar = Scorecard.LastHalfPar + Scorecard.FirstHalfPar;

            return View(Scorecard);
        }

        public ActionResult Scorecard()
            //(int userid, DateTime timestart)
        {
            ScorecardModel Scorecard = new ScorecardModel();

            PostgresModels Database = new PostgresModels();
            DataTable allData = Database.SqlQuery("SELECT holes.id, tees.name, meters.meters, holes.par, holes.hcp FROM public.holes, public.meters, public.tees WHERE holes.id = meters.hole_id AND meters.tee_id = tees.id;", PostgresModels.list = new List<NpgsqlParameter>());
            Scorecard.AllData = allData;

            PostgresModels Database2 = new PostgresModels();
            DataTable first = Database2.SqlQuery("SELECT SUM(par) ::integer FROM holes WHERE id BETWEEN 1 AND 9;", PostgresModels.list = new List<NpgsqlParameter>());
            foreach (DataRow dr in first.Rows)
            {
                Scorecard.FirstHalfPar = (int)dr["sum"];
            }

            PostgresModels Database3 = new PostgresModels();
            DataTable last = Database3.SqlQuery("SELECT SUM(par) ::integer FROM holes WHERE id BETWEEN 10 AND 18;", PostgresModels.list = new List<NpgsqlParameter>());
            foreach (DataRow dr2 in last.Rows)
            {
                Scorecard.LastHalfPar = (int)dr2["sum"];
            }

            Scorecard.TotalPar = Scorecard.LastHalfPar + Scorecard.FirstHalfPar;

            PostgresModels Database4 = new PostgresModels();
            DataTable member = Database4.SqlQuery("SELECT id, firstname, lastname, hcp, golfid, gender FROM members where id = 1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                //new NpgsqlParameter("@id", userid)
            });
            foreach (DataRow dr in member.Rows)
            {
                Scorecard.ScorecardMember.ID = (int)dr["id"];
                Scorecard.ScorecardMember.Firstname = (string)dr["firstname"];
                Scorecard.ScorecardMember.Lastname = (string)dr["lastname"];
                Scorecard.ScorecardMember.HCP = (double)dr["hcp"];
                Scorecard.ScorecardMember.GolfID = (string)dr["golfid"];
                Scorecard.ScorecardMember.Gender = (int)dr["gender"];
            }

            PostgresModels Database5 = new PostgresModels();
            DataTable ball = Database5.SqlQuery("SELECT balls.userid, reservations.timestart, members.firstname, members.lastname FROM reservations, balls, members WHERE balls.reservationid = reservations.id AND members.id = balls.userid AND reservations.timestart = '2017-03-13 13:10:00'", PostgresModels.list = new List<NpgsqlParameter>()
            {
                //new NpgsqlParameter("@timestart", timestart)
            });
            foreach (DataRow dr2 in ball.Rows)
            {
                ScorecardModel Guest = new ScorecardModel();
                Guest.ScorecardReservation.User = (int)dr2["userid"];
                Scorecard.ScorecardReservation.Timestart = (DateTime)dr2["timestart"];
                Guest.ScorecardReservation.Firstname = (string)dr2["firstname"];
                Guest.ScorecardReservation.Lastname = (string)dr2["lastname"];
                Scorecard.Guestlist.Add(Guest);
            }

            return View(Scorecard);
        }
    }
}