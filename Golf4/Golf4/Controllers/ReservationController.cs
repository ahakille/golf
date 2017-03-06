﻿using System;
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
            ReservationModels Reservation = new ReservationModels();     
            DataTable RBD = new DataTable();
            try
            {
                {
                    PostgresModels Database = new PostgresModels();
                    {
                        {
                            RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.user_id as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.golfid as \"mgi\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment \"mpa\" FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id WHERE date(timestart) = current_date ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>());
                        }
                    }
                    List<ReservationModels> reservationlist = new List<ReservationModels>();
                    foreach (DataRow dr in RBD.Rows)
                    {
                        Reservation.MemberID = (int)dr["mid"];
                        Reservation.MemberHCP = (double)dr["mh"];
                        Reservation.MemberGolfID = dr["mgi"].ToString();
                        Reservation.MemberGender = (int)dr["mg"];
                        Reservation.ID = (int)dr["rid"];
                        Reservation.Timestart = Convert.ToDateTime(dr["rts"]);
                        Reservation.Timeend = Convert.ToDateTime(dr["rte"]);
                        Reservation.Closed = (bool)dr["rc"];
                        Reservation.User = (int)dr["ru"];
                        reservationlist.Add(Reservation);
                    }
                    ViewBag.List = reservationlist;
                }
                Reservation.datepicker = DateTime.Now.Date.ToShortDateString();
                return View(Reservation);
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
            string chosendate = values["datepicker"];
            DataTable RBD = new DataTable();
            try
            {
                
                {
                    PostgresModels Database = new PostgresModels();
                    {
                        RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.user_id as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.golfid as \"mgi\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment \"mpa\" FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id WHERE date(timestart) = @chosendate ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                            new NpgsqlParameter("@chosendate", Convert.ToDateTime(chosendate)),
                        });
                    }
                    List<ReservationModels> reservationlist2 = new List<ReservationModels>();
                    foreach (DataRow dr in RBD.Rows)
                    {
                        ReservationModels Reservation = new ReservationModels();
                        Reservation.MemberID = (int)dr["mid"];
                        Reservation.MemberHCP = (double)dr["mh"];
                        Reservation.MemberGolfID = dr["mgi"].ToString();
                        Reservation.MemberGender = (int)dr["mg"];
                        Reservation.ID = (int)dr["rid"];
                        Reservation.Timestart = Convert.ToDateTime(dr["rts"]);
                        Reservation.Timeend = Convert.ToDateTime(dr["rte"]);
                        Reservation.Closed = (bool)dr["rc"];
                        Reservation.User = (int)dr["ru"];
                        reservationlist2.Add(Reservation);
                    }
                    ViewBag.List = reservationlist2;
                }
                ReservationModels selecteddate = new ReservationModels();
                selecteddate.datepicker = chosendate;
                return View(selecteddate);
            }
            catch
            {
                return View();
            }
        }
        // GET: Reservation/Create
        public ActionResult Create()
        {
            DateTime time = DateTime.Now;
            ReservationModels.CreatereservationModel model = new ReservationModels.CreatereservationModel();
            var id = User.Identity.Name;
            PostgresModels sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT members.id, members.firstname,members.lastname, members.address,members.postalcode,members.city,members.email,members.telephone,members.hcp,members.golfid,membercategories.category,genders.gender  FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id where members.id = @par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", Convert.ToInt16(id))
            });
            foreach (DataRow dr in dt.Rows)
            {
                model.ID = (int)dr["id"];
                model.Firstname = (string)dr["firstname"];
                model.Lastname = (string)dr["lastname"];
                model.GolfID = (string)dr["golfid"];
                model.HCP = (Double)dr["hcp"];
                model.Gender = (string)dr["gender"];
            }
            model.Timestart = time;
            return View(model);
        }

        // POST: Reservation/Create
        [HttpPost]
        public ActionResult Create(ReservationModels.CreatereservationModel reservation)
        {
            try
            {
                {
                    PostgresModels Database = new PostgresModels();
                    Database.SqlNonQuery("INSERT INTO reservations(timestart, timeend, closed, user_id) VALUES(@timestart, @timeend, @closed, @user);", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", reservation.Timestart),
                        new NpgsqlParameter("@timeend", reservation.Timestart),
                        new NpgsqlParameter("@closed", reservation.Closed),
                        new NpgsqlParameter("@user", reservation.ID)
                        });
                    Database = new PostgresModels();
                    DataTable dt= Database.SqlQuery("SELECT id from reservations WHERE user_id=@user AND timestart=@timestart", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", reservation.Timestart),
                        new NpgsqlParameter("@user", reservation.ID)
                        });
                     int id= 0;
                    foreach (DataRow item in dt.Rows)
                    {
                        id = (int)item["id"];
                    }
                    Database = new PostgresModels();
                    Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@user, @reservationid)", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@reservationid", id),
                        new NpgsqlParameter("@user", reservation.ID)
                        });

                }

                return RedirectToAction("reservation/Index");
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
