using Golf4.Models;
using Golf4.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Owin;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using Npgsql;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data;

namespace Golf4.Controllers
{
    public class UserController : Controller
    {
       // Loginsidan
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Loginfunktionen
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserModels.LoginViewModel model, string returnUrl)
        {
            // Kontrollerar så att de rätt format!
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);
            }
            
            PostgresModels sql = new PostgresModels();
            var userid =sql.SqlQuery("SELECT members.id, login.accounttype FROM members INNER JOIN login ON members.id=login.userid WHERE email =@par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", model.Email)

            });
            string id=null, type =null;
            foreach (DataRow item in userid.Rows)
            {
                 id= item["id"].ToString();
                 type= item["accounttype"].ToString();
            }

            if (id == null)
            {
                return View("Användarnamn eller lösenordstämmer inte");
            }
            else
            {
            UserModels password = new UserModels();
            bool result = password.AuthenticationUser(model.Password, id);
            if (result)
            {
                var identity = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, id),
            new Claim(ClaimTypes.Email, "xtian@email.com"),
            new Claim(ClaimTypes.Role, type) }, "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
            }
            }







            // I Fungerar inte riktigt.. Kommer försöka jobba runt!

            // TODO: Add insert logic here

            return RedirectToAction("Index");

        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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



      
    }
}
