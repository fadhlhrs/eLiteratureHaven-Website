using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
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
       
        public ActionResult Admin_users()
        {
            if(Session["role"].ToString() == "admin")
            {
                return View(db.users.ToList());
            }
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Admin_transactions()
        {
            return View();
        }

        public ActionResult Add_book()
        {
            return View();
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
        public ActionResult Edit_book([Bind(Include = "id, title, author, publisher, publication year, description, category, genre, image_path, pdf_path")] books book, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var cover_image_name = Path.GetFileName(imageFile.FileName);
                    var cover_image_folder_path = Server.MapPath("~/Content/Cover_images");
                    var cover_image_path = Path.Combine(cover_image_folder_path, cover_image_name);
                    imageFile.SaveAs(cover_image_path);
                    book.image_path = cover_image_name;
                }

                if (pdfFile != null && pdfFile.ContentLength > 0)
                {
                    var pdfFileName = Path.GetFileName(pdfFile.FileName);
                    var pdfFolderPath = Server.MapPath("~/Content/Uploads/PDFs");
                    var pdfFilePath = Path.Combine(pdfFolderPath, pdfFileName);
                    pdfFile.SaveAs(pdfFilePath);
                    book.pdf_path = pdfFileName; // Store only the file name
                }

                db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin_books");
            }
            return View(book);
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