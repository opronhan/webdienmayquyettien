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
    public class CashBillDetailsController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: Admin/CashBillDetails
        public PartialViewResult Index()
        {
            if (Session["cashdetails"] == null)
                Session["cashdetails"] = new List<CashBillDetail>();
            return PartialView(Session["cashdetails"]);
        }
        public int getSalePrice(int Product_ID)
        {
            return db.Products.Find(Product_ID).SalePrice;
        }



        // GET: Admin/CashBillDetails/Details/5
        public ActionResult Details(int id)
        {
            CashBillDetail model = db.CashBillDetails.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.Quantity = new SelectList(db.CashBillDetails, "BillID", "Quantity", model.BillID);
            ViewBag.SalePrice = new SelectList(db.CashBillDetails, "BillID", "SalePrice", model.BillID);
            return View(model);
        }

        // GET: Admin/CashBillDetails/Create
        public PartialViewResult Create()
        {
            ViewBag.Product_ID = new SelectList(db.Products, "ID", "ProductName");
            var model = new CashBillDetail();
            model.BillID = 0;
            model.Quantity = 1;
            return PartialView(model);  
        }

        // POST: Admin/CashBillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(CashBillDetail model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Environment.TickCount;
                model.Product = db.Products.Find(model.ProductID);
                var cashdetails = Session["cashdetails"] as List<CashBillDetail>;
                if (cashdetails == null)
                {
                    cashdetails = new List<CashBillDetail>();
                }
                
                cashdetails.Add(model);
                Session["cashdetails"] = cashdetails;
                return RedirectToAction("Create", "CashBillAdmin");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", model.ProductID);
            return View("Create", model);
        }

        // GET: Admin/CashBillDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBillDetail cashBillDetail = db.CashBillDetails.Find(id);
            if (cashBillDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillID = new SelectList(db.CashBills, "ID", "CustomerName", cashBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashBillDetail.ProductID);
            return View(cashBillDetail);
        }

        // POST: Admin/CashBillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CashBillDetail cashBillDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashBillDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashBillDetail.ProductID);
            return View(cashBillDetail);
        }

        // GET: Admin/CashBillDetails/Delete/5
        public ActionResult Delete(int id)
        {
            var cashdetails = Session["cashdetails"] as List<CashBillDetail>;
            cashdetails = cashdetails.Except(cashdetails.Where(c => c.ID == id)).ToList();
            Session["cashdetails"] = cashdetails;
            return RedirectToAction("Create", "CashBillAdmin");
        }

        // POST: Admin/CashBillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBillDetail cashBillDetail = db.CashBillDetails.Find(id);
            db.CashBillDetails.Remove(cashBillDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
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
