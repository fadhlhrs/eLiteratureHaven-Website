using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLiteratureHaven.Models;


namespace eLiteratureHaven.Controllers
{
    public class AdminController : Controller
    {
        private dbContext db = new dbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Admin_books()
        {
            if (Session["role"].ToString() == "admin")
            {
                return View(db.books.ToList());
            }
            return RedirectToAction("Login", "Home");
            
        }
        public ActionResult Admin_categories()
        {
            return View();
        }
        public ActionResult Admin_users()
        {
            return View();
        }
        public ActionResult Add_book()
        {
            return View();
        }
        public ActionResult Edit_book()
        {
            return View();
        }
        public ActionResult Add_user()
        {
            return View();
        }
        public ActionResult Edit_user()
        {
            return View();
        }
    }
}