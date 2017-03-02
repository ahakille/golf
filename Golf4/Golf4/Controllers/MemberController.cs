using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf4.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Npgsql;

namespace Golf4.Controllers
{
    [AllowAnonymous]
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            ClaimsIdentity i = new ClaimsIdentity();
            PostgresModels sql = new PostgresModels();
            var dt = sql.SqlQuery("SELECT members.id, members.firstname, members.address,members.postalcode,members.city,members.email,members.telephone,members.hcp,members.golfid,membercategories.category,genders.gender  FROM members INNER JOIN membercategories ON members.membercategory = membercategories.id Inner JOIN genders ON members.gender = genders.id where members.id = '1'", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", "1")

            });
            string test = i.Name; 

            return View();
        }

        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Models.MemberModels.MembersViewModel model)
        {
          int test=  model.ID;
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            
            try
            {
                

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
