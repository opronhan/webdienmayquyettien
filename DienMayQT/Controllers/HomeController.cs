using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DienMayQT.Models;

namespace DienMayQT.Controllers
{
    public class HomeController : Controller
    {
        DmQT09Entities db = new DmQT09Entities();
        public ActionResult Index()
        {
            var product = db.Products.OrderByDescending(x => x.ID).ToList();
            return View(product);
        }
        public ActionResult Info(int id)
        {
            Product model = db.Products.Find(id);
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", model.ProductTypeID);
            return View(model);
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
        public ActionResult Tintuc()
        {
            return View();
        }
    }
}