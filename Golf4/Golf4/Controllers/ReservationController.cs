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

        // GET: All reservations for specific day
        [HttpGet]
        public ActionResult Index()
        {
            DataTable RBD = new DataTable();
            try
            {
                {
                    PostgresModels Database = new PostgresModels();
                    {
                        RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.user as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.golfid as \"mgi\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment \"mpa\" FROM reservations INNER JOIN balls ON balls.reservationid = reservations.id INNER JOIN members ON balls.userid = members.id WHERE date(timestart) = current_date ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                            //new NpgsqlParameter("@timestart", DateTime.Now),
                        });
                    }
                    List<MemberModels> reservationlist = new List<MemberModels>();
                    foreach (DataRow dr in RBD.Rows)
                    {                    
                        MemberModels Member = new MemberModels();                                             
                        Member.ID = (int)dr["mid"];
                        Member.HCP = (double)dr["mh"];
                        Member.GolfID = dr["mgi"].ToString();
                        Member.Gender = (int)dr["mg"];
                        Member.Reservation.ID = (int)dr["rid"];
                        Member.Reservation.Timestart = Convert.ToDateTime(dr["rts"]);
                        Member.Reservation.Timeend = Convert.ToDateTime(dr["rte"]);
                        Member.Reservation.Closed = (bool)dr["rc"];
                        Member.Reservation.User = (int)dr["ru"];
                        reservationlist.Add(Member);
                    }
                    ViewBag.List = reservationlist;
                }
                return View();
            }
            catch
            {
                return View();
            }            
        }

        // Post: Change day
        [HttpPost]
        public ActionResult Index(FormCollection values)
        {
            string test = values["datepicker"];
            return null;
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
