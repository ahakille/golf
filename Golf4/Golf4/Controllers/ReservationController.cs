using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using NpgsqlTypes;
using Golf4.Models;
using System.Data;

namespace Golf4.Controllers
{
    [AllowAnonymous]

    public class ReservationController : Controller
    {

        // GET: Indexsidan
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Current time
        //public DateTime GetTime()
        //    {
        //    DateTime time = new DateTime();
        //    time = System.DateTime.Now;
        //    return time;
        //    }

        // GET: All reservations for specific day
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            DataTable RBD = new DataTable();
            try
            {
                {
                    PostgresModels Database = new PostgresModels();
                    {

                        RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.user as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment \"mpa\" FROM reservations INNER JOIN balls ON balls.reservationid = reservations.id INNER JOIN members ON balls.userid = members.id WHERE date(timestart) = @timestart", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", DateTime.Now),
                        });
                    }
                    List<MemberModels> reservationlist = new List<MemberModels>();
                    foreach (DataRow dr in RBD.Rows)
                    {
                        ReservationModels r = new ReservationModels();
                        MemberModels m = new MemberModels();

                        r.ID = (int)dr["rid"];
                        r.Timestart = Convert.ToDateTime(dr["rts"]);
                        r.Timeend = Convert.ToDateTime(dr["rte"]);
                        r.Closed = (bool)dr["rc"];
                        r.User = (int)dr["ru"];
                        //balls uid = (int)dr["bu"];
                        //balls rid = (int)dr["bi"];
                        m.ID = (int)dr["mid"];
                        m.Firstname = dr["mf"].ToString();
                        m.Lastname = dr["ml"].ToString();
                        m.Address = dr["ma"].ToString();
                        m.Postalcode = dr["mp"].ToString();
                        m.City = dr["mc"].ToString();
                        m.Email = dr["me"].ToString();
                        m.Telephone = dr["mt"].ToString();
                        m.HCP = (double)dr["mh"];
                        m.GolfID = dr["mg"].ToString();
                        m.Gender = (int)dr["mg"];
                        m.Membercategory = (int)dr["mct"];
                        m.Payment = (bool)dr["mpa"];

                        //reservationlist.Add(r);
                        //reservationlist.Add(m);
                    }

                    //int hour = timestart.Hour;
                    //int minute = timestart.Minute;
                    //string hourtext = hour.ToString() + minute.ToString();
                    //if (hourtext != null)
                    //{
                    //    ViewBag.hourtext = gender + " " + golfid + " " + hcp; ;
                    //}

                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Reservation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservation/Create
        [HttpPost]
        public ActionResult CreateReservation(ReservationModels reservation)
        {
            try
            {
                {
                    PostgresModels Database = new PostgresModels();
                    Database.SqlNonQuery("INSERT INTO reservation(timestart, timeend, closed, user) VALUES(@timestart, @timeend, @closed, @user)", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", reservation.Timestart),
                        new NpgsqlParameter("@timeend", reservation.Timeend),
                        new NpgsqlParameter("@closed", reservation.Closed),
                        new NpgsqlParameter("@user", reservation.ID)
                        });
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reservation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reservation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reservation/Delete/5
        [HttpPost]
        public ActionResult DeleteReservation(int reservationid)
        {
            try
            {
                // TODO: Add delete logic here
                {
                    PostgresModels Database = new PostgresModels();
                    Database.SqlNonQuery("DELETE FROM balls WHERE reservationid = @reservationid; DELETE FROM reservation WHERE id = @reservationid", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@reservationid", reservationid),
            });
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Ball/Create
        [HttpPost]
        public ActionResult CreateBall(List<int> memberid, int reservationid)
        {
            try
            {
                {
                    PostgresModels Database = new PostgresModels();
                    foreach (int userid in memberid)
                    {
                        Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@userid, @reservationid)", PostgresModels.list = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@userid", userid),
                            new NpgsqlParameter("@reservationid", reservationid)
                            });
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Ball/Delete
        [HttpPost]
        public ActionResult DeleteBall(int reservationid, int userid)
        {
            try
            {
                {
                    PostgresModels Database = new PostgresModels();
                    Database.SqlNonQuery("DELETE FROM balls WHERE reservationid = @reservationid AND userid = @userid", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@reservationid", reservationid),
                        new NpgsqlParameter("@userid", userid)
                        });
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
