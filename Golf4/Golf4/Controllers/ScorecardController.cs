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
        public ActionResult Scorecard()
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

            //PostgresModels Database4 = new PostgresModels();
            //DataTable holes = Database2.SqlQuery("SELECT holes.id, tees.name, meters.meters, holes.par, holes.hcp FROM public.holes, public.meters, public.tees WHERE holes.id = meters.hole_id AND meters.tee_id = tees.id;", PostgresModels.list = new List<NpgsqlParameter>());

            // PostgresModels Database3 = new PostgresModels();
            // DataTable tees = Database3.SqlQuery("SELECT * FROM tees", PostgresModels.list = new List<NpgsqlParameter>());

            // Scorecard.Tees = tees;

            return View(Scorecard);
        }
        public ActionResult Index()
        {

            return View();
        }
    }
}