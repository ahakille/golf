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
            //(int userid, DateTime timestart, int teeid)
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
            foreach (DataRow dr3 in member.Rows)
            {
                Scorecard.ScorecardMember.ID = (int)dr3["id"];
                Scorecard.ScorecardMember.Firstname = (string)dr3["firstname"];
                Scorecard.ScorecardMember.Lastname = (string)dr3["lastname"];
                Scorecard.ScorecardMember.HCP = (double)dr3["hcp"];
                Scorecard.ScorecardMember.GolfID = (string)dr3["golfid"];
                Scorecard.ScorecardMember.Gender = (int)dr3["gender"];
            }

            PostgresModels Database5 = new PostgresModels();
            DataTable ball = Database5.SqlQuery("SELECT balls.userid, reservations.timestart, members.firstname, members.lastname FROM reservations, balls, members WHERE balls.reservationid = reservations.id AND members.id = balls.userid AND reservations.timestart = '2017-03-13 13:10:00'", PostgresModels.list = new List<NpgsqlParameter>()
            {
                //new NpgsqlParameter("@timestart", timestart)
            });
            foreach (DataRow dr4 in ball.Rows)
            {
                ScorecardModel Guest = new ScorecardModel();
                Guest.ScorecardReservation.User = (int)dr4["userid"];
                Scorecard.ScorecardReservation.Timestart = (DateTime)dr4["timestart"];
                Guest.ScorecardReservation.Firstname = (string)dr4["firstname"];
                Guest.ScorecardReservation.Lastname = (string)dr4["lastname"];
                Scorecard.Guestlist.Add(Guest);
            }

            PostgresModels Database6 = new PostgresModels();
            DataTable tees = Database6.SqlQuery("SELECT * FROM tees where id = 1", PostgresModels.list = new List<NpgsqlParameter>());
            foreach (DataRow dr5 in tees.Rows)
            {
                Scorecard.TeeID = (int)dr5["id"];
                Scorecard.TeeName = (string)dr5["name"];
                Scorecard.WomanCR = (double)dr5["woman_cr"];
                Scorecard.WomanSlope = (int)dr5["woman_slope"];
                Scorecard.ManCR = (double)dr5["man_cr"];
                Scorecard.ManSlope = (int)dr5["man_slope"];
            }

            int coursepar = Scorecard.FirstHalfPar + Scorecard.LastHalfPar;

            if (Scorecard.ScorecardMember.Gender == 1)
            {
                double ASM = Scorecard.ScorecardMember.HCP * (Scorecard.ManSlope / 113) + (Scorecard.ManCR - coursepar);
                double RASM = Math.Round(ASM, MidpointRounding.AwayFromZero);
                int strokes = Convert.ToInt32(RASM);
                Scorecard.Strokes = strokes;
                int holes = 18;
                if (strokes > holes)
                {
                    holes %= strokes;
                }
            }

            else
            {
                double ASW = Scorecard.ScorecardMember.HCP * (Scorecard.WomanSlope / 113) + (Scorecard.WomanCR - coursepar);
                double RASW = Math.Round(ASW, MidpointRounding.AwayFromZero);
                int strokes = Convert.ToInt32(RASW);
                Scorecard.Strokes = strokes;
                int holes = 18;
                if (strokes > holes)
                {
                    holes %= strokes;
                }
            }
            
            return View(Scorecard);
        }
    }
}