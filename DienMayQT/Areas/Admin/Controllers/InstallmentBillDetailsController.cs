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
    public class InstallmentBillDetailsController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: Admin/InstallmentBillDetails
        public PartialViewResult Index()
        {
            if (Session["installment"] == null)
                Session["installment"] = new List<InstallmentBillDetail>();
            return PartialView(Session["installment"]);
        }
        public int getInsPrice(int Product_ID)
        {
            return db.Products.Find(Product_ID).InstallmentPrice;
        }

        // GET: Admin/InstallmentBillDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            if (installmentBillDetail == null)
            {
                return HttpNotFound();
            }
            return View(installmentBillDetail);
        }

        // GET: Admin/InstallmentBillDetails/Create
        public ActionResult Create()
        {
            ViewBag.Product_ID = new SelectList(db.Products, "ID", "ProductName");
            var model = new InstallmentBillDetail();
            model.BillID = 0;
            model.Quantity = 1;
            return PartialView(model);
        }

        // POST: Admin/InstallmentBillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstallmentBillDetail model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Environment.TickCount;
                model.Product = db.Products.Find(model.ProductID);
                var installment = Session["installment"] as List<InstallmentBillDetail>;
                if (installment == null)
                {
                    installment = new List<InstallmentBillDetail>();
                }

                installment.Add(model);
                Session["cashdetails"] = installment;
                return RedirectToAction("Create", "InstallmentBillsAdmin");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", model.ProductID);
            return View(model);
        }

        // GET: Admin/InstallmentBillDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            if (installmentBillDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillID = new SelectList(db.InstallmentBills, "ID", "BillCode", installmentBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", installmentBillDetail.ProductID);
            return View(installmentBillDetail);
        }

        // POST: Admin/InstallmentBillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BillID,ProductID,Quantity,InstallmentPrice")] InstallmentBillDetail installmentBillDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(installmentBillDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillID = new SelectList(db.InstallmentBills, "ID", "BillCode", installmentBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", installmentBillDetail.ProductID);
            return View(installmentBillDetail);
        }

        // GET: Admin/InstallmentBillDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            if (installmentBillDetail == null)
            {
                return HttpNotFound();
            }
            return View(installmentBillDetail);
        }

        // POST: Admin/InstallmentBillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            db.InstallmentBillDetails.Remove(installmentBillDetail);
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
