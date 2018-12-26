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
    public class CashBillAdminController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: /Admin/CashBillAdmin/
        public ActionResult Index()
        {
            var cashbill = db.CashBills.OrderByDescending(x => x.ID).ToList();
            if (Session["Username"] != null && Session["NVKDPer"] != null)
            {
                return View(cashbill);
            }
            else
            {
                return RedirectToAction("Login", "LoginAdmin");
            }
            
        }
        

        // GET: /Admin/CashBillAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/CashBillAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult Create(CashBillDetail[] billDetail,CashBill model)
        {
            if (ModelState.IsValid && billDetail != null)
            {
                int grandTotal = 0;
                foreach (var item in billDetail)
                {
                    
                    CashBillDetail O = new CashBillDetail();
                    Product thisProduct = db.Products.Where(p => p.ID == item.ProductID).FirstOrDefault();
                    O.BillID = model.ID;
                    O.ProductID = item.ProductID;
                    O.Quantity = item.Quantity;
                    O.SalePrice = thisProduct.SalePrice * item.Quantity;
                    grandTotal = grandTotal + (thisProduct.SalePrice * item.Quantity);
                    db.CashBillDetails.Add(O);
                }
                model.GrandTotal = grandTotal;
                db.CashBills.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /Admin/CashBillAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashbill = db.CashBills.Find(id);
            if (cashbill == null)
            {
                return HttpNotFound();
            }
            return View(cashbill);
        }

        // POST: /Admin/CashBillAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,BillCode,CustomerName,PhoneNumber,Address,Date,Shipper,Note,GrandTotal")] CashBill cashbill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashbill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cashbill);
        }

        // GET: /Admin/CashBillAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashbill = db.CashBills.Find(id);
            if (cashbill == null)
            {
                return HttpNotFound();
            }
            return View(cashbill);
        }

        // POST: /Admin/CashBillAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBill cashbill = db.CashBills.Find(id);
            db.CashBills.Remove(cashbill);
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
