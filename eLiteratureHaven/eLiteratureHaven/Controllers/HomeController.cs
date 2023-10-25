using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLiteratureHaven.Models;

namespace eLiteratureHaven.Controllers
{
    public class HomeController : Controller
    {
        private dbContext db = new dbContext();
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(users user)
        {

            try
            {
                var obj = db.users.Where(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["username"] = obj.Username.ToString();
                    Session["role"] = obj.Role.ToString();
                    Session["userID"] = obj.ID.ToString();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
            }
            TempData["msg"] = "<script>alert('Username or Password is incorrect');</script>";

            return View(user);
        }

        public ActionResult Register()
        {
            return View(/*"Register"*/);
        }
        /*[HttpPost]
        public ActionResult Register(users user)
        {
            if (ModelState.IsValid)
            {

                var obj = db.users.Where(a => a.Username.Equals(user.Username)).FirstOrDefault();
                if (obj != null)
                {
                    TempData["msg"] = "<script>alert('Username already taken');</script>";
                }
                else
                {
                    user.Role = "member";
                    db.users.Add(user);
                    db.SaveChanges();
                    TempData["msg"] = "<script>alert('Registration success!');</script>";
                }

            }
            return View();
        }*/
    }
}