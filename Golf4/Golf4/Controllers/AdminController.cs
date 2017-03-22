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
            int id = Convert.ToInt32(Request.QueryString["member"]);
            AdminModels model = new AdminModels();
            model.Membercategory = model.CollectMembercategory();
            model.Gender = model.CollectGender();
            DataTable dt = model.GetMember(id);
            foreach (DataRow item in dt.Rows)
            {
                model.ID = (int)item["id"];
                model.Firstname = (string)item["firstname"];
                model.Lastname = (string)item["lastname"];
                model.Adress = (string)item["address"];
                model.Postalcode = (string)item["postalcode"];
                model.City = (string)item["city"];
                model.Telephone = (string)item["telephone"];
                model.Email = (string)item["email"];
                model.GolfID = (string)item["golfid"];
                model.HCP = (double)item["hcp"];
                model.membercategoryselected = (int)item["membercategory"];
                model.Genderselected = (int)item["gender"];
            }

            return View(model);
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            try
            {
                MemberModels.MembersViewModel Member = new MemberModels.MembersViewModel()
                {
                    Firstname = form["Firstname"],
                    Lastname = form["Lastname"],
                    Adress = form["Adress"],
                    Postalcode = form["postalcode"],
                    City = form["City"],
                    Telephone = form["Telephone"],
                    Email = form["Email"],
                    HCP = Convert.ToInt16(form["HCP"]),
                    Membercategory = form["Membercategory"],
                    Gender = form["Gender"],
                    Payment = Convert.ToBoolean(form["Payment"])
                };

                MemberModels.CreateMember(Member);
                
                return RedirectToAction("members");
            }

            catch
            {

                int id = Convert.ToInt32(Request.QueryString["member"]);
                AdminModels model = new AdminModels();
                model.Membercategory = model.CollectMembercategory();
                model.Gender = model.CollectGender();
                model.Password = "Lösenord";
                DataTable dt = model.GetMember(id);
                foreach (DataRow item in dt.Rows)
                {
                    model.ID = (int)item["id"];
                    model.Firstname = (string)item["firstname"];
                    model.Lastname = (string)item["lastname"];
                    model.Adress = (string)item["address"];
                    model.Postalcode = (string)item["postalcode"];
                    model.City = (string)item["city"];
                    model.Telephone = (string)item["telephone"];
                    model.Email = (string)item["email"];
                    model.GolfID = (string)item["golfid"];
                    model.HCP = (double)item["hcp"];
                    model.membercategoryselected = (int)item["membercategory"];
                    model.Genderselected = (int)item["gender"];
                }                                
                return View(model);
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit()
        {
            int id = Convert.ToInt32(Request.QueryString["member"]);
            AdminModels model = new AdminModels();
            model.Membercategory= model.CollectMembercategory();
            model.Gender = model.CollectGender();
            DataTable dt = model.GetMember(id);
            foreach (DataRow item in dt.Rows)
            {
                model.ID = (int)item["id"];
                model.Firstname = (string)item["firstname"];
                model.Lastname = (string)item["lastname"];
                model.Adress = (string)item["address"];
                model.Postalcode = (string)item["postalcode"];
                model.City = (string)item["city"];
                model.Telephone = (string)item["telephone"];
                model.Email = (string)item["email"];
                model.GolfID = (string)item["golfid"];
                model.HCP = (double)item["hcp"];
                model.membercategoryselected = (int)item["membercategory"];
                model.Genderselected = (int)item["gender"];
            }

            return View(model);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                
                AdminModels model = new AdminModels();
                model.UpdateMemberAdmin(collection["Firstname"], collection["Lastname"], collection["Adress"], collection["Postalcode"], collection["City"], collection["Email"], collection["Telephone"],Convert.ToDouble(collection["HCP"]),Convert.ToInt32(collection["Gender"]),Convert.ToInt32(collection["Membercategory"]), collection["Golfid"], Convert.ToBoolean(collection["Payment"]), Convert.ToInt32(collection["ID"]));

                return RedirectToAction("members");
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
