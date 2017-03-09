using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf4.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Npgsql;
using System.Data;

namespace Golf4.Controllers
{
    
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            List<MemberModels.MembersViewModel> reservationList = new List<MemberModels.MembersViewModel>();
            MemberModels.MembersViewModel model = new MemberModels.MembersViewModel();
            int id = Convert.ToInt16(User.Identity.Name);
            PostgresModels sql = new PostgresModels();
            //Ändrat till modelnen
            model.Timestart = sql.SqlQuery("SELECT reservations.id as \"Reservation\", reservations.timestart as \"Tillfälle\", reservations.id as \"Avboka\" FROM reservations JOIN balls ON balls.reservationid=reservations.id WHERE balls.userid=@identity ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@identity",Convert.ToInt16(id)),
             });
            //foreach (DataRow res in reserv.Rows)
            //{
            //    model.Timestart = (DateTime)res["timestart"];
            //}
            sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT members.id, members.firstname,members.lastname, members.address,members.postalcode,members.city,members.email,members.telephone,members.hcp,members.golfid,membercategories.category,genders.gender  FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id where members.id = @par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", id)
            });
            foreach (DataRow dr in dt.Rows)
            {
                model.ID =(int)dr["id"];
                model.Firstname = (string)dr["firstname"];
                model.Lastname = (string)dr["lastname"];
                model.Adress = (string)dr["address"];
                model.Postalcode = (string)dr["postalcode"];
                model.City = (string)dr["city"];
                model.Email = (string)dr["email"];
                model.Telephone = (string)dr["telephone"];
                model.GolfID = (string)dr["golfid"];
                model.HCP = (Double)dr["hcp"];
                model.Membercategory = (string)dr["category"];
                model.Gender = (string)dr["gender"];
            }

            TempData["user"] = model;

            return View(model);
        }


        // GET: Member/Edit/
        [HttpGet]
        public ActionResult Edit()
        {
          
            return View(TempData["user"]);
        }

        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            int id = Convert.ToInt16(User.Identity.Name);
            string fname = collection["Firstname"], lname = collection["lastname"], address = collection["adress"], postalcode = collection["postalcode"], city = collection["city"], email = collection["email"], telephone = collection["telephone"], hcp = collection["hcp"];
            double hcp1 = Convert.ToDouble(hcp);
            try
            {
                
                PostgresModels sql = new PostgresModels();
                sql.SqlNonQuery("UPDATE members SET firstname=@par2,lastname =@par3, address=@par4,postalcode=@par5,city=@par6,email=@par7,telephone=@par8,hcp=@par9 WHERE id =@par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par2", fname),
                new NpgsqlParameter("@par3", lname),
                new NpgsqlParameter("@par4", address),
                new NpgsqlParameter("@par5", postalcode),
                new NpgsqlParameter("@par6", city),
                new NpgsqlParameter("@par7", email),
                new NpgsqlParameter("@par8", telephone),
                new NpgsqlParameter("@par9", hcp1),
                new NpgsqlParameter("@par1", id)
            });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Member/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
