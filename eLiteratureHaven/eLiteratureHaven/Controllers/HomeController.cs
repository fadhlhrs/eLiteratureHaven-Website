using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
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

        public ActionResult Home()
        {
            var booksList = new List<books>();

            // Create instances of different books and add them to the list
            var book1 = new books
            {
                id = 1,
                title = "Harry Potter and the Sorcerers Stone",
                image_path = "Cover_Harry Potter and the Sorcerers Stone.jpg"
            };
            booksList.Add(book1);

            var book2 = new books
            {
                id = 2,
                title = "No Logo",
                image_path = "Cover_No Logo.jpg"
            };
            booksList.Add(book2);

            // Add more books as needed...

            // Pass the list of books to the view
            return View(booksList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(users user)
        {
                var obj = db.users.Where(a => a.username.Equals(user.username) && a.password.Equals(user.password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["username"] = obj.username.ToString();
                    Session["role"] = obj.role.ToString();
                    Session["id"] = obj.id.ToString();
                    return RedirectToAction("Home");
                }
                else

            TempData["msg"] = "<script>alert('Username or Password is incorrect');</script>";

            return View(user);
        }

        public ActionResult Register()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Category(string selectedCategory, string selectedGenres)
        {
            string[] selectedItems = Request.Form.GetValues("selectedItems");


            if (selectedGenres != null && selectedGenres.Any())
            {
                string selectedGenresString = string.Join(", ", selectedGenres);

                var result = db.books.SqlQuery($"SELECT * FROM books WHERE Genres LIKE '%{selectedGenresString}%'").ToList();
                return View(result);
            }

            return View();
        }
        public ActionResult User_page()
        {
            return View();
        }
        public ActionResult Book_details()
        {
            return View();
        }
        public ActionResult Search_result()
        {
            return View();
        }
        public ActionResult Category_result()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(users user)
        {
            if (ModelState.IsValid)
            {

                var obj = db.users.Where(a => a.username.Equals(user.username)).FirstOrDefault();
                if (obj != null)
                {
                    TempData["msg"] = "<script>alert('Username already taken');</script>";
                }
                else
                {
                    user.role = "member";
                    db.users.Add(user);
                    db.SaveChanges();
                    TempData["msg"] = "<script>alert('Registration success!');</script>";
                }

            }
            return View();
        }
    }
}