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

            // PostgresModels Database2 = new PostgresModels();
            // DataTable tees = Database2.SqlQuery("SELECT * FROM tees", PostgresModels.list = new List<NpgsqlParameter>());

            Scorecard.AllData = allData;
            // Scorecard.Tees = tees;

            return View(Scorecard);
        }
        public ActionResult Index()
        {

            return View();
        }
    }
}