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
    public class AccountController : Controller
    {
       // Loginsidan
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Loginfunktionen
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(AccountModels.LoginViewModel model, string returnUrl)
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
                ModelState.AddModelError("", "Fel löenord");
                return View(model);
            }
            else
            {
                AccountModels password = new AccountModels();
                bool result = password.AuthenticationUser(model.Password, id);
                if (result)
                {
                    var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role, type) }, "ApplicationCookie");

                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(identity);

                    if (type == "2")
                    { 
                        return Redirect("Admin/Index");
                    }
                    else
                    {
                        return Redirect("Member/index/");
                    }
            }
                else
                {
                    ModelState.AddModelError("", "Fel löenord");
                    return View(model);
                }
            }

           

        }
       
        // GET: users/newnuser
        [Authorize(Roles = "2")]
        public ActionResult Newuser()
        {
            return View();
        }

        // POST: User/newuser
        [Authorize(Roles = "2")]
        [HttpPost]
        public ActionResult Newuser(AccountModels.NewuserViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);
            }
            try
            {
                AccountModels User = new AccountModels();
                Tuple<byte[],byte[]>password = User.Generatepass(model.Password);
                PostgresModels sql = new PostgresModels();
                sql.SqlNonQuery("UPDATE login set salt= @par2, key =@par3 WHERE userid =@par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", model.Userid),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@par3", password.Item2)
            });

                return RedirectToAction("newuser");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }



    }
}
