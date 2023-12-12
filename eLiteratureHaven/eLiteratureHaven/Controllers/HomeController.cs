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
        public ActionResult Home(string home_search)
        {
            string[] searchTerms = Request.Form.GetValues("home_search");
            if (searchTerms != null && searchTerms.Any())
            {
                string searchTermsString = string.Join("+", searchTerms);
                return RedirectToAction("Search_result", new { searchTermsString});
            };

            return View();

        }
        public ActionResult Search_result(string searchTermsString)
        {
            if (searchTermsString != null)
            {
                var searchTerms = searchTermsString.Split('+').ToList();

                var filteredBooks = db.books
                    .Where(book => searchTerms.All(term =>
                    book.title.Contains(term) ||
                    book.author.Contains(term)
                    ))
                    .ToList();

                return View("~/Views/Home/Search_result.cshtml", filteredBooks);
            }
            
            return View();
        }

        [HttpPost]
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

        public ActionResult Category()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Category(string selectedCategory, string selectedGenres)
        {
            string[] selectedItems1 = Request.Form.GetValues("selectedGenres");
            string[] selectedItems2 = Request.Form.GetValues("selectedCategory");
            if (selectedItems1 != null && selectedItems1.Any())
            {
                string selectedGenresString = string.Join("+", selectedItems1);
                string selectedCategoryString = string.Join("+", selectedItems2);
                return RedirectToAction("Category_result",new { genres = selectedGenresString, category = selectedCategoryString});
            };
            
            return View();

        }

        
        public ActionResult Category_result(string genres, string category)
        {
            var genresList = genres.Split('+').ToList();

            var filteredBooks = db.books
                .Where(book => book.category == category &&
                    genresList.All(genre => book.genre.Contains(genre)))
                .ToList();

            return View("~/Views/Home/Category_result.cshtml", filteredBooks);
        }

        public ActionResult User_page()
        {
            return View();
        }
        public ActionResult Book_details()
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
                    user.role = "customer";
                    db.users.Add(user);
                    db.SaveChanges();
                    TempData["msg"] = "<script>alert('Registration success!');</script>";
                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("username");
            Session.Remove("role");
            Session.Remove("id");

            return RedirectToAction("login");
        }

    }
}