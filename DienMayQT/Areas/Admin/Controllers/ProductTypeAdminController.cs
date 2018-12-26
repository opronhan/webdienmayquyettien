using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQT.Models;

namespace DienMayQT.Areas.Admin.Controllers
{
    public class ProductTypeAdminController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: /Admin/ProductTypeAdmin/
        public ActionResult Index()
        {
            var productType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            if (Session["Username"] != null && Session["NVKDPer"] != null)
            {
                return View(productType);
            }
            else
            {
                return RedirectToAction("Login", "LoginAdmin");
            }
        }

        // GET: /Admin/ProductTypeAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/ProductTypeAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductType model)
        {
            checkLoaiSP(model);
            if (ModelState.IsValid)
            {
                db.ProductTypes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Admin/ProductTypeAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType producttype = db.ProductTypes.Find(id);
            if (producttype == null)
            {
                return HttpNotFound();
            }
            return View(producttype);
        }

        // POST: /Admin/ProductTypeAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductType model)
        {
            checkLoaiSP(model);
            if (ModelState.IsValid)
            {
                ProductType thisProductType = db.ProductTypes.Where(p => p.ID == model.ID).FirstOrDefault();
                db.Entry(thisProductType).CurrentValues.SetValues(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        private void checkLoaiSP(ProductType model)
        {
            var productType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            foreach (var item in productType)
            {
                if (item.ProductTypeCode == model.ProductTypeCode && item.ID != model.ID)
                {
                    ModelState.AddModelError("ProductTypeCode", "Mã loại sản phẩm đã tồn tại !");
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
