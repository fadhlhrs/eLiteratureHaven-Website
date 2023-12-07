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
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Admin_home()
        {
            return View();
        }
    }
}