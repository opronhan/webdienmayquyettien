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
            if (Session["installBillDetail"] == null)
                Session["installBillDetail"] = new List<InstallmentBillDetail>();
            return PartialView(Session["installBillDetail"]);
        }
        public int InstallmentPrice(int ProductID)
        {
            return db.Products.Find(ProductID).InstallmentPrice;
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
        public PartialViewResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");
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
        public ActionResult Create2(InstallmentBillDetail model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Environment.TickCount;
                model.Product = db.Products.Find(model.ProductID);
                var installBillDetail = Session["installBillDetail"] as List<InstallmentBillDetail>;
                if (installBillDetail == null)
                {
                    installBillDetail = new List<InstallmentBillDetail>();
                }

                installBillDetail.Add(model);
                Session["installBillDetail"] = installBillDetail;
                return RedirectToAction("Create", "InstallmentBillsAdmin");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", model.ProductID);
            return View("Create",model);
        }

        // GET: Admin/InstallmentBillDetails/Edit/5
        public PartialViewResult Edit(int id)
        {

            List<InstallmentBillDetail> details = db.InstallmentBillDetails.Where(c => c.BillID == id).ToList();
            if (Session["installBillDetail"] == null)
                Session["installBillDetail"] = new List<InstallmentBillDetail>();
            ViewBag.details = details;
            ViewBag.installBillDetail = Session["installBillDetail"];
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(InstallmentBillDetail installmentBillDetail)
        {
            if (ModelState.IsValid)
            {
                installmentBillDetail.ID = Environment.TickCount;
                installmentBillDetail.Product = db.Products.Find(installmentBillDetail.ProductID);
                var installBillDetail = Session["installBillDetail"] as List<InstallmentBillDetail>;
                if (installBillDetail == null)
                    installBillDetail = new List<InstallmentBillDetail>();
                installBillDetail.Add(installmentBillDetail);
                Session["installBillDetail"] = installBillDetail;
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", installmentBillDetail.ProductID);
            return View("Create", installmentBillDetail);
        }


        // POST: Admin/InstallmentBillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstallmentBillDetail installmentBillDetail)
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
        public ActionResult Delete(int id)
        {
            var details = Session["installBillDetail"] as List<InstallmentBillDetail>;
            details = details.Except(details.Where(c => c.ID == id)).ToList();
            Session["installBillDetail"] = details;
            return RedirectToAction("Create", "InstallmentBillsAdmin");
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
