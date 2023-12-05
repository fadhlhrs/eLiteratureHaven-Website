using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLiteratureHaven.Models;

namespace eLiteratureHaven.Controllers
{
    public class BookController : Controller
    {
        public ActionResult Book_details()
        {
            return View();
        }
    }
}