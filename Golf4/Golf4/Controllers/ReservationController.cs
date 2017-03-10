using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using NpgsqlTypes;
using Golf4.Models;
using System.Data;
using System.Web.UI;

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
                PostgresModels Database = new PostgresModels();
                {
                    {
                        RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.user_id as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.golfid as \"mgi\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment \"mpa\" FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id WHERE date(timestart) = current_date OR reservations.closed = TRUE ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>());
                    }
                }
                List<ReservationModels> reservationlist = new List<ReservationModels>();
                foreach (DataRow dr in RBD.Rows)
                {
                    ReservationModels Reservation2 = new ReservationModels();
                    Reservation2.MemberID = (int)dr["mid"];
                    Reservation2.MemberHCP = (double)dr["mh"];
                    Reservation2.MemberGolfID = dr["mgi"].ToString();
                    Reservation2.MemberGender = (int)dr["mg"];
                    Reservation2.ID = (int)dr["rid"];
                    Reservation2.Timestart = Convert.ToDateTime(dr["rts"]);
                    Reservation2.Timeend = Convert.ToDateTime(dr["rte"]);
                    Reservation2.Closed = (bool)dr["rc"];
                    Reservation2.User = (int)dr["ru"];
                    reservationlist.Add(Reservation2);
                }

                ViewBag.List = reservationlist;
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
                        RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.user_id as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.golfid as \"mgi\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment \"mpa\" FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id WHERE date(timestart) = @chosendate OR reservations.closed = TRUE ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
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
                    //ViewData.Clear();
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
            int CountGolfers = Int32.Parse(Request.QueryString["countgolfers"]);
            ReservationModels.CreatereservationModel model = new ReservationModels.CreatereservationModel();
            model.CountGolfers = CountGolfers;
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
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
            
            TempData["time"] = model.Timestart;
            return View(model);
        }

        // POST: Reservation/Create
        [HttpPost]
        public ActionResult Create(ReservationModels.CreatereservationModel model)
        {
            int user2 = 0, user3 = 0, user4 = 0;
            double user2hcp = 0, user3hcp = 0, user4hcp = 0;
            int id = 0;
            PostgresModels Database = new PostgresModels();
            model.Timestart = (DateTime)TempData["time"];
            var test = model.ID;
            PostgresModels sql2 = new PostgresModels();
            DataTable dt2 = sql2.SqlQuery("SELECT SUM(hcp) FROM balls, members, reservations WHERE balls.userid = members.id AND reservations.id = balls.reservationid AND timestart = @timestart", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@timestart", model.Timestart)
            });
            if (dt2 != null)
            {
                foreach (DataRow dr in dt2.Rows)
                {
                    model.TotalHCP = (double)dr["sum"];
                }
            }
            try
            {
                {
                    //bool checktime = Database.Check("SELECT CASE WHEN EXISTS(SELECT 1 FROM reservations WHERE reservations.timestart = @timestart)THEN CAST(1 AS BIT) ELSE CAST (0 AS BIT) END", PostgresModels.list = new List<NpgsqlParameter>()
                    //    {
                    //    new NpgsqlParameter("@timestart", model.Timestart),
                    //    });

                    //if (!checktime)
                    //{
                    ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();

                    DataTable dt = Database.SqlQuery("INSERT INTO reservations(timestart, timeend, closed, user_id) VALUES(@timestart, @timeend, @closed, @user) returning id;", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", model.Timestart),
                        new NpgsqlParameter("@timeend", model.Timestart),
                        new NpgsqlParameter("@closed", model.Closed),
                        new NpgsqlParameter("@user", model.ID)
                        });
                    foreach (DataRow dr in dt.Rows)
                    {
                        id = (int)dr["id"];
                    }
                    //}
                 //   id = makebooking.MakeReservations(model.Timestart, model.Timestart, model.Closed, model.ID);
                    try
                    {
                        Database = new PostgresModels();

                        DataTable dt3 = Database.SqlQuery("SELECT members.id, members.golfid, members.hcp FROM members WHERE golfid = @golfer2 OR golfid = @golfer3 OR golfid = @golfer4", PostgresModels.list = new List<NpgsqlParameter>()
                                    {
                                    new NpgsqlParameter("@golfer2", model.GolfID2),
                                    new NpgsqlParameter("@golfer3", model.GolfID3),
                                    new NpgsqlParameter("@golfer4", model.GolfID4),
                              });

                        foreach (DataRow dr in dt3.Rows)
                        {
                            ReservationModels Golfer = new ReservationModels();
                            Golfer.MemberID = (int)dr["id"];
                            Golfer.MemberGolfID = (string)dr["golfid"];
                            Golfer.MemberHCP = (double)dr["hcp"];
                            if (model.GolfID2 == Golfer.MemberGolfID)
                            {
                                user2 = Golfer.MemberID;
                                user2hcp = Golfer.MemberHCP;
                            }
                            if (model.GolfID3 == Golfer.MemberGolfID)
                            {
                                user3 = Golfer.MemberID;
                                user3hcp = Golfer.MemberHCP;
                            }
                            if (model.GolfID4 == Golfer.MemberGolfID)
                            {
                                user4 = Golfer.MemberID;
                                user4hcp = Golfer.MemberHCP;
                            }
                        }
                    }
                    catch
                    {

                    }
                    if ((model.TotalHCP + model.HCP + user2hcp + user3hcp + user4hcp) <= 120)
                    {
                        Database = new PostgresModels();
                        Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@user, @reservationid);", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                            new NpgsqlParameter("@reservationid", id),
                            new NpgsqlParameter("@user", model.ID),
                            });

                        if (user2 != 0)
                        {
                            Database = new PostgresModels();
                            Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@user2, @reservationid);", PostgresModels.list = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@reservationid", id),
                            new NpgsqlParameter("@user2", user2),
                            });
                        }
                        if (user3 != 0)
                        {
                            Database = new PostgresModels();
                            Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@user3, @reservationid);", PostgresModels.list = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@reservationid", id),
                            new NpgsqlParameter("@user3", user3),
                            });
                        }
                        if (user4 != 0)
                        {
                            Database = new PostgresModels();
                            Database.SqlNonQuery("INSERT INTO balls(userid, reservationid) VALUES(@user4, @reservationid);", PostgresModels.list = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@reservationid", id),
                            new NpgsqlParameter("@user4", user4),
                            });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Summan av HCP för samtliga spelare på bokad tid får ej överstiga 120!");
                        return View(model);
                    }
                    
                }
                    return RedirectToAction("/Index");
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
        
        public ActionResult DeleteReservation()
        {
            ReservationModels.AdminViewModel model = new ReservationModels.AdminViewModel();
            ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
            model.ID = Convert.ToInt16(Request.QueryString["member"]);
            try
            {

                Tuple<bool,int> ids =  makebooking.CheckReservationUser(model.Timestart, model.ID);
                if (ids.Item1)
                {
                    makebooking.DeleteReservation(ids.Item2);
                }
                else
                {
                    makebooking.DeleteBoll(ids.Item2, model.ID);
                }
               
                
                return RedirectToAction("admin", "reservation", new { validdate = model.Timestart });
            }
            catch
            {
                return View();
            }
        }
        // POST: Ball/Delete
        
        public ActionResult DeleteBall()
        {
            try
            {
                {
                   
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Admin ()
        {
            ReservationModels.AdminViewModel model = new ReservationModels.AdminViewModel();
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
          //  model.Timestart = Convert.ToDateTime("2017-03-07 13:00:00");
            PostgresModels sql = new PostgresModels();
            model.medlemmar = sql.SqlQuery("SELECT members.golfid , members.firstname,members.lastname,members.hcp,membercategories.category,genders.gender,members.id FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id", PostgresModels.list = new List<NpgsqlParameter>()
            { });
            sql = new PostgresModels();
            model.reservation = sql.SqlQuery("SELECT  members.golfid as GolfID , members.firstname as förnamn, members.lastname as efternamn, members.email as email, members.telephone as telefon, members.hcp as HCP, genders.gender as Kön, membercategories.category as Medlemskategori, members.id as id FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id LEFT JOIN genders ON members.gender = genders.id  LEFT JOIN membercategories ON members.membercategory = membercategories.id WHERE reservations.timestart = @timestart", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@timestart", model.Timestart),
                        });
            TempData["time"] = model.Timestart;
            return View(model);
        }
        
        public ActionResult Adminadd()
        {
            ReservationModels.AdminViewModel model = new ReservationModels.AdminViewModel();
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
            model.ID = Convert.ToInt16(Request.QueryString["member"]);
            ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
            int reservation_id =makebooking.MakeReservations(model.Timestart, model.Timestart, model.Closed, model.ID);
            makebooking.MakeReservationBalls(reservation_id, model.ID);
           
            return RedirectToAction("admin", "reservation", new { validdate = model.Timestart });
        }
        public ActionResult Adminaddboll()
        {
            ReservationModels.AdminViewModel model = new ReservationModels.AdminViewModel();
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
            model.ID = Convert.ToInt16(Request.QueryString["member"]);
            ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
            int reservation_id = makebooking.CollectReservationId(model.Timestart);
            if(reservation_id == 0)
            {
                return RedirectToAction("admin", "reservation", new { validdate = model.Timestart });
            }
            else
            {
                makebooking.MakeReservationBalls(reservation_id, model.ID);
                return RedirectToAction("admin", "reservation", new { validdate = model.Timestart });
            }

        }
        public ActionResult Adminedit()
        {
            return RedirectToAction("admin");
        }

        
        public ActionResult deleteResv(MemberModels.MembersViewModel Member)
        {
            DateTime start = new DateTime(2018,02,28,11,00,00);
            DateTime slut = new DateTime(2018,03,09,16,00,00);

            ReservationModels.CloseGolfCourseView.CloseCourse(start, slut, 1);
            //ReservationModels.RemoveReservation(Member.ID, Member.ReservationID);
            return RedirectToAction("index", "Member");
        }

        public ActionResult CloseGolfCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CloseGolfCourse(FormCollection form)
        {
            return View();
        }
    }
}
