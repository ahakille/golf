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
    public class AdminController : Controller
    {
        [Authorize(Roles = "2")]
        // GET: Admin
        public ActionResult Index()
        {
            MemberModels member = new MemberModels();
            AdminModels.Adminviewmodel model = new AdminModels.Adminviewmodel();
            DataTable dt = member.CollectOneMember(Convert.ToInt32(User.Identity.Name));
            foreach (DataRow dr in dt.Rows)
            {
                
                model.Firstname = (string)dr["firstname"];
                model.Lastname = (string)dr["lastname"];
            }

            return View(model);
        }

        [Authorize(Roles = "2")]
        public ActionResult Members()
        {
            List<AdminModels.Adminviewmodel> list = new List<AdminModels.Adminviewmodel>();
            AdminModels.Adminviewmodel model = new AdminModels.Adminviewmodel();
            MemberModels member = new MemberModels();
            return View(member.CollectAllMembers());
        }

        [Authorize(Roles = "2")]
        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [Authorize(Roles = "2")]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
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

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
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

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
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
