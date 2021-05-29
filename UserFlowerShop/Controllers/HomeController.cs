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
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("AccessError");
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //string FormHash = GetMD5Hash(model.Password);
                var user = db.Users.FirstOrDefault(u => u.Mail == model.Mail && u.Password == model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем не существует");
                }
                else
                {

                    FormsAuthentication.SetAuthCookie(model.Mail, false);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index");

                }
            }
            return View(model);
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