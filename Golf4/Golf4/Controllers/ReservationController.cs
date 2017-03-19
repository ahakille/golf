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
                    RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.contest as \"rco\", reservations.user_id as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.golfid as \"mgi\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment as \"mpa\", balls.checkedin as \"chk\" FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id WHERE date(timestart) = @chosendate OR reservations.closed = TRUE ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        DateTime.Now.Hour < 18 ? new NpgsqlParameter("@chosendate", DateTime.Today) : new NpgsqlParameter("@chosendate", DateTime.Today.AddDays(1))
                        });
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
                    Reservation2.Contest = (bool)dr["rco"];
                    Reservation2.User = (int)dr["ru"];
                    Reservation2.CheckedIn = (bool)dr["chk"];
                    reservationlist.Add(Reservation2);
                }

                ViewBag.List = reservationlist;

                Reservation.datepicker = DateTime.Now.Hour < 18 ? DateTime.Now.Date.ToShortDateString() : DateTime.Now.Date.AddDays(1).ToShortDateString();
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
                        RBD = Database.SqlQuery("SELECT reservations.id as \"rid\", reservations.timestart as \"rts\", reservations.timeend as \"rte\", reservations.closed as \"rc\", reservations.contest as \"rco\", reservations.user_id as \"ru\", balls.userid as \"bu\", balls.reservationid as \"bi\", members.id as \"mid\", members.firstname as \"mf\", members.lastname as \"ml\", members.address as \"ma\", members.postalcode as \"mp\", members.city as \"mc\", members.email as \"me\", members.telephone as \"mt\", members.hcp as \"mh\", members.golfid as \"mgi\", members.gender as \"mg\", members.membercategory as \"mct\", members.payment as \"mpa\", balls.checkedin as \"chk\" FROM reservations JOIN balls ON balls.reservationid = reservations.id JOIN members ON balls.userid = members.id WHERE date(timestart) = @chosendate OR reservations.closed = TRUE ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
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
                        Reservation.Contest = (bool)dr["rco"];
                        Reservation.User = (int)dr["ru"];
                        Reservation.CheckedIn = (bool)dr["chk"];
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
            DataTable dt = sql.SqlQuery("SELECT members.id, members.firstname,members.lastname, members.address,members.postalcode,members.city,members.email,members.telephone,members.hcp,members.golfid,membercategories.category,genders.gender FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id where members.id = @par1", PostgresModels.list = new List<NpgsqlParameter>()
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
            
            model.Timestart = (DateTime)TempData["time"];
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
                    PostgresModels Database1 = new PostgresModels();
                    DataTable dt = Database1.SqlQuery("INSERT INTO reservations(timestart, timeend, closed, contest, user_id) VALUES(@timestart, @timeend, @closed, FALSE, @user) returning id;", PostgresModels.list = new List<NpgsqlParameter>()
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

                string guestgolfer = "";
                string golfer = "ofrivillig";
                
                if (model.Guest)
                {
                        guestgolfer = "Gäst";
                }
                    PostgresModels Database3 = new PostgresModels();
                    DataTable dt3 = Database3.SqlQuery("SELECT members.id, members.golfid, members.hcp FROM members WHERE golfid = @golfer2 OR golfid = @golfer3 OR golfid = @golfer4", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        model.GolfID2 == null ? (model.Guest ? new NpgsqlParameter("@golfer2", guestgolfer) : new NpgsqlParameter("@golfer2", golfer)) : new NpgsqlParameter("@golfer2", model.GolfID2),
                        model.GolfID3 == null ? new NpgsqlParameter("@golfer3", golfer) : new NpgsqlParameter("@golfer3", model.GolfID3),
                        model.GolfID4 == null ? new NpgsqlParameter("@golfer4", golfer) : new NpgsqlParameter("@golfer4", model.GolfID4),
                        });

                    foreach (DataRow dr in dt3.Rows)
                    {
                        ReservationModels Golfer = new ReservationModels();
                        Golfer.MemberID = (int)dr["id"];
                        Golfer.MemberGolfID = (string)dr["golfid"];
                        Golfer.MemberHCP = (double)dr["hcp"];

                        if (model.GolfID2 == Golfer.MemberGolfID || Golfer.MemberGolfID == "Gäst")
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

                bool check = false;
                DataTable test = new DataTable();
                PostgresModels Database2 = new PostgresModels();
                {
                    test = Database2.SqlQuery("SELECT EXISTS(SELECT reservations.timestart, balls.userid FROM balls, reservations WHERE balls.reservationid = reservations.id AND balls.userid != 1002 AND balls.userid = @user1 OR balls.userid = @user2 OR balls.userid =  @user3 OR balls.userid = @user3 OR balls.userid = @user4 AND DATE(reservations.timestart) = @timestart) AS \"check\"", PostgresModels.list = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@user1", model.ID),
                            new NpgsqlParameter("@user2", user2),
                            new NpgsqlParameter("@user3", user3),
                            new NpgsqlParameter("@user4", user4),
                            new NpgsqlParameter("@timestart", model.Timestart)
                            });
                }
                foreach (DataRow dr2 in test.Rows)
                {
                    check = (bool)dr2["check"];
                }

                if (check)
                {
                    ModelState.AddModelError("", "Spelare får endast vara inbokade en gång per dag!");
                    return View(model);
                }

                else
                {

                    if ((model.TotalHCP + model.HCP + user2hcp + user3hcp + user4hcp) <= 120)
                    {
                        PostgresModels Database4 = new PostgresModels();
                        Database4.SqlNonQuery("INSERT INTO balls(userid, reservationid, checkedin) VALUES(@user, @reservationid, FALSE);", PostgresModels.list = new List<NpgsqlParameter>()
                                {
                                    new NpgsqlParameter("@reservationid", id),
                                    new NpgsqlParameter("@user", model.ID),
                                    });

                        if (user2 != 0)
                        {
                            PostgresModels Database5 = new PostgresModels();
                            Database5.SqlNonQuery("INSERT INTO balls(userid, reservationid, checkedin) VALUES(@user2, @reservationid, FALSE);", PostgresModels.list = new List<NpgsqlParameter>()
                                    {
                                    new NpgsqlParameter("@reservationid", id),
                                    new NpgsqlParameter("@user2", user2),
                                    });
                        }
                        if (user3 != 0)
                        {
                            PostgresModels Database6 = new PostgresModels();
                            Database6.SqlNonQuery("INSERT INTO balls(userid, reservationid, checkedin) VALUES(@user3, @reservationid, FALSE);", PostgresModels.list = new List<NpgsqlParameter>()
                                    {
                                    new NpgsqlParameter("@reservationid", id),
                                    new NpgsqlParameter("@user3", user3),
                                    });
                        }
                        if (user4 != 0)
                        {
                            PostgresModels Database7 = new PostgresModels();
                            Database7.SqlNonQuery("INSERT INTO balls(userid, reservationid, checkedin) VALUES(@user4, @reservationid, FALSE);", PostgresModels.list = new List<NpgsqlParameter>()
                                    {
                                    new NpgsqlParameter("@reservationid", id),
                                    new NpgsqlParameter("@user4", user4),
                                    });
                        }

                        List<MemberModels.MembersViewModel> Members = EmailModels.GetEmail(id);
                        EmailModels.SendEmail("tim592096@gmail.com", "zave12ave", Members, "Bokad", " Denna tid har du blivit bokad på");
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
        [Authorize(Roles = "2")]
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
        [Authorize(Roles = "2")]
        public ActionResult Admin ()
        {
            ReservationModels.AdminViewModel model = new ReservationModels.AdminViewModel();
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
            ReservationModels.CheckInMember checkin = new ReservationModels.CheckInMember();
            List<MemberModels.MembersViewModel> list = new List<MemberModels.MembersViewModel>();
            PostgresModels sql = new PostgresModels();
            model.medlemmar = sql.SqlQuery("SELECT members.golfid as \"GolfID\", members.firstname as \"Förnamn\",members.lastname as \"Efternamn\",members.hcp as \"HCP\",membercategories.category as \"Medlemskategori\",genders.gender as \"Kön\",members.id as \"Välj Åtgärd\"FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id", PostgresModels.list = new List<NpgsqlParameter>()
            { });
            list = checkin.GetMembersInReservation(model.Timestart);
            ViewBag.list = list;
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Välj tee", Value = "1" });
            items.Add(new SelectListItem { Text = "Gul", Value = "3" });
            items.Add(new SelectListItem { Text = "Röd", Value = "2" });           
          //  ViewBag.selecteditem = "Välj tee";
            ViewBag.teelist = items;
        //    TempData["time"] = model.Timestart;
            return View(model);
        }
        [Authorize(Roles = "2")]
        public ActionResult Adminadd()
        {
            ReservationModels.AdminViewModel model = new ReservationModels.AdminViewModel();
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
            model.ID = Convert.ToInt16(Request.QueryString["member"]);
            ReservationModels.MakeBooking makebooking = new ReservationModels.MakeBooking();
            int reservation_id = makebooking.MakeReservations(model.Timestart, model.Timestart, model.Closed, model.Contest, model.ID);
            makebooking.MakeReservationBalls(reservation_id, model.ID);
           
            return RedirectToAction("admin", "reservation", new { validdate = model.Timestart });
        }
        [Authorize(Roles = "2")]
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
        [Authorize(Roles = "2")]
        public ActionResult Adminedit()
        {
            return RedirectToAction("admin");
        }

        
        public ActionResult deleteResv()
        {
            int MemberID = Convert.ToInt16(User.Identity.Name);
            int ReservationID = Convert.ToInt32(Request.QueryString["ReservationID"]);
            List<MemberModels.MembersViewModel> members = EmailModels.GetEmail(ReservationID);
            EmailModels.SendEmail("tim592096@gmail.com", "zave12ave", members, "Avbokad", " tiden har blivit avbokad");
            ReservationModels.RemoveReservation(MemberID, ReservationID);
            return RedirectToAction("index", "Member");
        }

        public void test()
        {
            // test metod
            List<int> list = new List<int>() {0, 1, 3 };
            ContestModels.Contest.MembersInContestTimeSetting(list);
            //List<MemberModels.MembersViewModel> members = EmailModels.GetEmail(id);
            //EmailModels.SendEmail("tim592096@gmail.com", "zave12ave", members, "Avbokad", " Denna Tid har blivit avbokad");
        }

        public void test2()
        {
            // test metod
            List<MemberModels.MembersViewModel> members = EmailModels.GetEmail(Convert.ToDateTime("2017-03-09"), Convert.ToDateTime("2017-03-11"));
            EmailModels.SendEmail("tim592096@gmail.com", "zave12ave", members, "Stängning av banan", " Denna tid har blivit tyvär avbokad pga stängning av banan");
        }

        [HttpGet]
        public ActionResult CloseCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "2")]
        public ActionResult CloseCourse(FormCollection closeform)
        {
                int id_reservation = 0;
                string timestart = closeform["Timestart"];
                string timeend = closeform["Timeend"];

                List<MemberModels.MembersViewModel> members = EmailModels.GetEmail(Convert.ToDateTime(closeform["Timestart"]), Convert.ToDateTime(closeform["Timeend"]));

                EmailModels.SendEmail("tim592096@gmail.com", "zave12ave", members, "Stängning av banan", " Denna tid har blivit tyvär avbokad pga stängning av banan");

            PostgresModels Database = new PostgresModels();
                DataTable Table = Database.SqlQuery("UPDATE reservations SET closed = TRUE WHERE timestart BETWEEN @timestart AND @timeend", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@timestart", Convert.ToDateTime(timestart)),
                    new NpgsqlParameter("@timeend", Convert.ToDateTime(timeend)),
                });

                PostgresModels Database2 = new PostgresModels();
                DataTable dt = Database2.SqlQuery("INSERT INTO reservations(timestart, timeend, closed, contest, user_id) VALUES(@timestart, @timeend, TRUE, FALSE, @userid) returning id;", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@timestart", Convert.ToDateTime(timestart)),
                    new NpgsqlParameter("@timeend", Convert.ToDateTime(timeend)),
                    new NpgsqlParameter("@userid", Convert.ToInt16(User.Identity.Name))
                });

            foreach (DataRow dr in dt.Rows)
            {
                id_reservation = (int)dr["id"];
            }

            PostgresModels Database3 = new PostgresModels();
            Database3.SqlNonQuery("INSERT INTO balls(userid, reservationid, checkedin) VALUES(1002, @reservationid, FALSE);", PostgresModels.list = new List<NpgsqlParameter>()
                        {
                        new NpgsqlParameter("@reservationid", id_reservation)
                    });

            return View();
        }
        [Authorize(Roles = "2")]
        public ActionResult CheckInMember()
        {
            ReservationModels.AdminViewModel model = new ReservationModels.AdminViewModel();
            model.Timestart = Convert.ToDateTime(Request.QueryString["validdate"]);
            model.ID = Convert.ToInt16(Request.QueryString["member"]);
            ReservationModels.CheckInMember checkin = new ReservationModels.CheckInMember();
            checkin.CheckInAllMember(model.Timestart, model.ID);
            

            return RedirectToAction("admin", "reservation", new { validdate = model.Timestart });
        }
    }
}
