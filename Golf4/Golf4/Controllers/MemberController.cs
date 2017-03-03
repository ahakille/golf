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
    [AllowAnonymous]
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            MemberModels.MembersViewModel model = new MemberModels.MembersViewModel();
            int test = 2;
            ClaimsIdentity i = new ClaimsIdentity();
            PostgresModels sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT members.id, members.firstname,members.lastname, members.address,members.postalcode,members.city,members.email,members.telephone,members.hcp,members.golfid,membercategories.category,genders.gender  FROM members INNER JOIN membercategories ON members.membercategory = membercategories.id Inner JOIN genders ON members.gender = genders.id where members.id = @par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", test)
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


        // GET: Member/Edit/5
        [HttpGet]
        public ActionResult Edit()
        {
          
            return View(TempData["user"]);
        }

        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {

            string fname = collection["Firstname"],lname = collection[3];
            try
            {
                
                PostgresModels sql = new PostgresModels();
                sql.SqlNonQuery("UPDATE members SET firstname=@par2, address=@par3,postalcode=@par4,city=@par5,email=@par6,telephone=@par7,hcp=@par8 WHERE id = '2'", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par2", fname),
                new NpgsqlParameter("@par3", lname),
                new NpgsqlParameter("@par4", collection[4]),
                new NpgsqlParameter("@par5", collection[5]),
                new NpgsqlParameter("@par6", collection[6]),
                new NpgsqlParameter("@par7", collection[7]),
                new NpgsqlParameter("@par8", collection[8]),
                new NpgsqlParameter("@par9", collection[9]),
                new NpgsqlParameter("@par1", collection[9])
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
