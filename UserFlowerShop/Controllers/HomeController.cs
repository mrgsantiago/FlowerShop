using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserFlowerShop.Models;

namespace UserFlowerShop.Controllers
{
    public class HomeController : Controller
    {
        private static FlowerEntities db = new FlowerEntities();
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult CheckAuthenticated()
        {
            if (User.Identity.IsAuthenticated) return Json(new { check = true });
            else return Json(new { check = false });

        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("AccessError");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Login(string Login, string Password)
        {
            
                //string FormHash = GetMD5Hash(model.Password);
                var user = db.Users.FirstOrDefault(u => u.Mail == Login && u.Password == Password);
                if (user == null)
                {
                    var login = db.Users.FirstOrDefault(u => u.Mail == Login);
                    bool LoginCheck = true, PasswordCheck = true;
                    if (login != null)
                    {
                        LoginCheck = true;
                        PasswordCheck = false;
                    }
                    else
                    {
                        PasswordCheck = true;
                        LoginCheck = false;
                    }
                    return Json(new { LoginCheck, PasswordCheck, AllResult = false });
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(Login, false);
                    //Session["Menu"] = null;
                    return Json(new { LoginCheck = "", PasswordCheck = "", AllResult = true });
                }

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}