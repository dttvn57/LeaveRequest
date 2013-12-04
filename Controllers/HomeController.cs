using LeaveRequest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveRequest.Controllers
{
    public class HomeController : Controller
    {
        RequestFormDB _db = new RequestFormDB();

        public ActionResult Index()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Confirm()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
