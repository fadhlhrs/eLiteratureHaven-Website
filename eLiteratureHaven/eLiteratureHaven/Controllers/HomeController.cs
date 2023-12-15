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
using System.Data.SqlClient;

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
            var bookIds = new List<int> { 1, 3, 9, 10, 11, 12, 13, 14 };

            // Query the database to get books with the specified ids
            var booksList = db.books
                .Where(b => bookIds.Contains(b.id))
                .Select(b => new // Anonymous type
        {
                    id = b.id,
                    title = b.title,
                    image_path = b.image_path,
                    author = b.author,
                    publication_year = b.publication_year
                })
                .ToList()
                .Select(b => new books // Creating instances of the 'books' class in memory
        {
                    id = b.id,
                    title = b.title,
                    image_path = b.image_path,
                    author = b.author,
                    publication_year = b.publication_year
                })
                .ToList();

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

            var viewModel = new book_transaction_viewmodel
            {
                books = book
            };

            if (Session["username"] != null)
            {
                string idString = (string)Session["id"];
                int userId = Int32.Parse(idString);
                int bookId = book.id;

                // Check if the user has rented the book
                // Your existing code...
                string sqlQuery = "SELECT TOP 1 transaction_status FROM transactions WHERE user_id = @userId AND book_id = @bookId ORDER BY create_date DESC";
                string transactionStatus = db.Database.SqlQuery<string>(sqlQuery,
                    new SqlParameter("userId", userId),
                    new SqlParameter("bookId", bookId)
                ).FirstOrDefault();
                // ... (rest of the code remains unchanged)
                

                string buttonText = "";

                if (transactionStatus == "pending")
                {
                    ViewBag.TransactionStatus = "pending";
                    buttonText = "Pending";
                }
                else if (transactionStatus == "processing")
                {
                    ViewBag.TransactionStatus = "processing";
                    buttonText = "Processing";
                }
                else if (transactionStatus == "rented")
                {
                    ViewBag.TransactionStatus = "rented";
                    buttonText = "Read";
                }
                else if (transactionStatus == "cancelled")
                {
                    ViewBag.TransactionStatus = "cancelled";
                }
                else if (transactionStatus == "due")
                {
                    ViewBag.TransactionStatus = "due";
                }
                ViewBag.ButtonText = buttonText;
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Rent_process(int books_id, int user_id, transactions transaction)
        {
            
                transaction.book_id = books_id;
                transaction.user_id = user_id;
                db.transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Payment", "Home", new { books_id, user_id });
            
        }

        [HttpPost]
        public ActionResult Payment_page(int bookId, int userId)
        {
            transactions transaction = new transactions
            {
                user_id = userId,
                book_id = bookId,
                transaction_status = "pending"
            };
            db.transactions.Add(transaction);
            db.SaveChanges();

            int newTransaction = transaction.id;

            return View(transaction);

        }


        public ActionResult ProcessPayment(int id, string paymentStatus, int book_id)
        {
            try
            {
                var transaction = db.transactions.FirstOrDefault(t => t.id == id);
                
                    if (paymentStatus == "processing")
                    {
                        // Handle payment success logic
                        transaction.transaction_status = "processing";
                        transaction.payment_date = DateTime.Now;
                    }
                    else if (paymentStatus == "cancelled")
                    {
                        // Handle payment cancellation logic
                        transaction.transaction_status = "cancelled";
                    }
                     
                    db.SaveChanges();

                    return RedirectToAction("Book_details", "Home", new { id = transaction.book_id });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return RedirectToAction("ErrorPage");
            }
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

        public ActionResult your_books()
        {
            int user_id = Convert.ToInt32(Session["id"]);

            DataTable dataTable = GetDataFromDatabase(user_id);

            return View(dataTable);
        }

        private DataTable GetDataFromDatabase(int user_id)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(@"Data Source=DRAGONFLY\SQLEXPRESS;Initial Catalog=eLiteratureHaven;Integrated Security=True;"))
            {
                connection.Open();

                string query = @"
            WITH RankedTransactions AS (
                SELECT
                    transactions.book_id,
                    transactions.user_id,
                    books.image_path,
                    books.title,
                    books.author,
                    books.publication_year,
                    books.category,
                    books.genre,
                    transactions.transaction_status,
                    ROW_NUMBER() OVER (PARTITION BY transactions.book_id, transactions.user_id ORDER BY transactions.create_date DESC) AS RowNum
                FROM
                    transactions
                    INNER JOIN books ON transactions.book_id = books.id
                WHERE
                    transactions.user_id = @UserId
                    AND transactions.transaction_status <> 'cancelled'
            )
            SELECT
                book_id,
                user_id,
                image_path,
                title,
                author,
                publication_year,
                category,
                genre,
                transaction_status
            FROM
                RankedTransactions
            WHERE
                RowNum = 1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", user_id);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }


    }
}