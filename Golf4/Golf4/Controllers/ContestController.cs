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
        // GET: Competition
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            PostgresModels Database = new PostgresModels();
            {
                dt = Database.SqlQuery("SELECT reservations.timestart, reservations.timeend, contests.name, contests.description, contests.closetime FROM reservations, contests WHERE reservations.id = contests.reservationid AND DATE(reservations.timestart) > CURRENT_DATE AND contests.closetime > @time", PostgresModels.list = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@time", DateTime.Now)
                    });
            }

            return View(dt);
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(ContestModels model)
        {
            return View();
        }
    }

}