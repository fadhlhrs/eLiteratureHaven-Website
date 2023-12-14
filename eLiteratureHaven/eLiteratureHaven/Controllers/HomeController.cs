using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Validation;
using System.Net;
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
            ViewBag.Message = "About Us";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us";

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

            var book3 = new books
            {
                id = 3,
                title = "Alice in Wonderland",
                image_path = "Cover_AliceInWonderland.jpg"
            };
            booksList.Add(book3);

            var book4 = new books
            {
                id = 4,
                title = "Freakonomics: A Rogue Economist Explores the Hidden Side of Everything",
                image_path = "Cover_Freakonomic.jpg"
            };
            booksList.Add(book4);

            var book5 = new books
            {
                id = 5,
                title = "The Sociological Imagination",
                image_path = "Cover_The Sociological Imagination.jpg"
            };
            booksList.Add(book5);

            var book6 = new books
            {
                id = 6,
                title = "The Lean Startup",
                image_path = "Cover_The Lean Startup.jpg"
            };
            booksList.Add(book6);

            var book7 = new books
            {
                id = 7,
                title = "The Hobbit",
                image_path = "Cover_The Hobbit.jpg"
            };
            booksList.Add(book7);

            var book8 = new books
            {
                id = 8,
                title = "The Lord of The Ring",
                image_path = "Cover_The Lord of The Ring The Return of The King.jpg"
            };
            booksList.Add(book8);

            var book9 = new books
            {
                id = 9,
                title = "The Maze Runner The Kill Order",
                image_path = "Cover_The Maze Runner The Kill Order.jpg"
            };
            booksList.Add(book9);

            var book10 = new books
            {
                id = 10,
                title = "Harry Potter and The Goblet of Fire",
                image_path = "Cover_Harry Potter and The Goblet of Fire.jpg"
            };
            booksList.Add(book10);

            var book11 = new books
            {
                id = 11,
                title = "The Maze Runner The Death Cure",
                image_path = "Cover_The Maze Runner The Death Cure.jpg"
            };
            booksList.Add(book11);

            var book12 = new books
            {
                id = 12,
                title = "Winnie The Pooh",
                image_path = "Cover_Winnie The Pooh.jpg"
            };
            booksList.Add(book12);

            var book13 = new books
            {
                id = 13,
                title = "Percy Jackson and The Olypians The Lightning Thief",
                image_path = "Cover_Percy Jackson and The Olypians The Lightning Thief.jpg"
            };
            booksList.Add(book13);

            var book14 = new books
            {
                id = 14,
                title = "Percy Jackson and The Olypians The Sea of The Monster",
                image_path = "Cover_Percy Jackson and The Olypians The Sea of The Monster.jpg"
            };
            booksList.Add(book14);

            var book15 = new books
            {
                id = 15,
                title = "How to Write Non-Fiction",
                image_path = "Cover_How to Write Non-Fiction.jpg"
            };
            booksList.Add(book15);

            var book16 = new books
            {
                id = 16,
                title = "Pocahontas",
                image_path = "Cover_Pocahontas.jpg"
            };
            booksList.Add(book16);

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
            string sessionId = (string)Session["id"];
            int id;

            if (int.TryParse(sessionId, out id))
            {
                var user = db.users.FirstOrDefault(u => u.id == id);
                if (user != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("Home", "Home");
        }

        public ActionResult Book_details(int id)
        {
            var book = db.books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
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

        public ActionResult PDF_viewer(int id)
         {
            string sessionId = (string)Session["id"];
            // Set SameSite=None; Secure for cross-site cookies sent from HTTPS pages
            HttpCookie myCookie = new HttpCookie(sessionId, sessionId);

            // Set SameSite=None; Secure for cross-site cookies sent from HTTPS pages
            myCookie.Secure = true; // Make sure to set this if your site is served over HTTPS

            // Manually set the SameSite attribute using Set-Cookie header
            Response.AppendHeader("Set-Cookie", $"{myCookie.Name}={myCookie.Value}; SameSite=None; Secure");


            var book = db.books.Find(id);
             if (book == null)
             {
                 return HttpNotFound();
             }

             return View(book);
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