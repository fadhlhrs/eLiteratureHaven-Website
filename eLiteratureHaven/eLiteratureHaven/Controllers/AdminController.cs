using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using eLiteratureHaven.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;


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
       
        public ActionResult Admin_users()
        {
            if(Session["role"].ToString() == "admin")
            {
                return View(db.users.ToList());
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Add_book()
        {
            books model = new books();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_book([Bind(Include = "id, title, author, publisher, publication_year, description, category, genre, image_path, pdf_path")] books book, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var imageFileName = Path.GetFileName(imageFile.FileName);
                    var imageFolderPath = Server.MapPath("~/Content/Cover_images");
                    var imageFilePath = Path.Combine(imageFolderPath, imageFileName);
                    imageFile.SaveAs(imageFilePath);
                    book.image_path = imageFileName; // Store file name with extension
                }

                if (pdfFile != null && pdfFile.ContentLength > 0)
                {
                    var pdfFileName = Path.GetFileName(pdfFile.FileName);
                    var pdfFileFolderPath = Server.MapPath("~/Content/PDF");
                    var pdfFilePath = Path.Combine(pdfFileFolderPath, pdfFileName);
                    pdfFile.SaveAs(pdfFilePath);
                    book.pdf_path = pdfFileName; // Store only the file name
                }

                db.books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Admin_books");
            }
            return View(book);
        }

        public ActionResult Edit_book(int id)
        {
            var book = db.books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_book([Bind(Include = "id, title, author, publisher, publication_year, description, category, genre, image_path, pdf_path")] books book, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var imageFileName = Path.GetFileName(imageFile.FileName);
                    var imageFolderPath = Server.MapPath("~/Content/Cover_images");
                    var imageFilePath = Path.Combine(imageFolderPath, imageFileName);
                    imageFile.SaveAs(imageFilePath);
                    book.image_path = imageFileName; // Store file name with extension
                }

                if (pdfFile != null && pdfFile.ContentLength > 0)
                {
                    var pdfFileName = Path.GetFileName(pdfFile.FileName);
                    var pdfFileFolderPath = Server.MapPath("~/Content/PDF");
                    var pdfFilePath = Path.Combine(pdfFileFolderPath, pdfFileName);
                    pdfFile.SaveAs(pdfFilePath);
                    book.pdf_path = pdfFileName; // Store file name with extension
                }

                db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin_books");
            }
            return View(book);
        }

        public ActionResult Delete_book(int id)
        {
            books book = db.books.Find(id);
            return View(book);
        }

        [HttpPost, ActionName("Delete_book")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                books book = db.books.Find(id);
                db.books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Admin_books");
            }
            catch
            {
                TempData["msg"] = "<script>alert('Delete unsuccessful');</script>";
            }
            return View();
        }

        public ActionResult Admin_transactions()
        {
            if (Session["role"].ToString() == "admin")
            {
                var sortedTransactions = db.transactions.OrderByDescending(t => t.create_date).ToList();
                return View(sortedTransactions);
            }
            return RedirectToAction("Login", "Home");

        }

        public ActionResult Edit_transaction(int id)
        {
            var transaction = db.transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }

        [HttpPost]
        public ActionResult Edit_transaction([Bind(Include = "id, due_date, transaction_status")]transactions transaction)
        {
            if (transaction.transaction_status == "rented")
            {
                string sql = "UPDATE transactions SET transaction_status = @transaction.transaction_status, due_date = DATEADD(day, 7, GETDATE()) WHERE id = @id";
                db.Database.ExecuteSqlCommand(sql, new SqlParameter("@transaction_status", "rented"), new SqlParameter("@id", transaction.id));
            }
            else
            {
                string sql = "UPDATE transactions SET transaction_status = @transaction_status WHERE id = @id";
                db.Database.ExecuteSqlCommand(sql, new SqlParameter("@transaction_status", transaction.transaction_status), new SqlParameter("@id", transaction.id));
            }

            return RedirectToAction("Admin_transactions");
        }

    }
}