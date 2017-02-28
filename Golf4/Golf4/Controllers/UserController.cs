using Golf4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;

namespace Golf4.Controllers
{
    public class UserController : Controller
    {
       // Loginsidan
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Loginfunktionen
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(UserModels.LoginViewModel model, string returnUrl)
        {
            // Kontrollerar så att de rätt format!
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);
            }
            // logik för att kontrollera mot databasen





            var identity = new ClaimsIdentity(new [] {
            new Claim(ClaimTypes.Email, model.Email),
            new Claim(ClaimTypes.Country, "Philippines")
                                 }, "ApplicationCookie");

            // I Fungerar inte riktigt.. Kommer försöka jobba runt!
            //  IAuthenticationManager;
            //var ctx = GetOwinContext(HttpContext);
            //var authManager = ctx.Authentication;
            //authManager.SignIn(identity);

            // TODO: Add insert logic here

            //    return RedirectToAction("Index");

            return View();
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
