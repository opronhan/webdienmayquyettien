using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQT.Models;
using System.Transactions;

namespace DienMayQT.Areas.Admin.Controllers
{
    public class CashBillAdminController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: /Admin/CashBillAdmin/
        public ActionResult Index()
        {
            if (Session["Username"] != null && Session["NVKDPer"] != null)
            {
                return View(db.CashBills.ToList());
            }
            else
            {
                return RedirectToAction("Login", "LoginAdmin");
            }
            
        }
        

        // GET: /Admin/CashBillAdmin/Create
        public ActionResult Create()    
        {
            return View(Session["cashBill"]);
        }

        // POST: /Admin/CashBillAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult Create(CashBill model)
        {
            if (ModelState.IsValid)
            {
                Session["cashBill"] = model;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2()
        {
            using (var scope = new TransactionScope())
                try
                {
                    var c = Session["cashBill"] as CashBill;
                    var d = Session["cashdetails"] as List<CashBillDetail>;
                    c.Date = DateTime.Now;
                    c.GrandTotal = (int)Session["total"];
                    db.CashBills.Add(c);
                    db.SaveChanges();

                    foreach (var details in d)
                    {
                        details.BillID = c.ID;
                        details.Product = null;
                        db.CashBillDetails.Add(details);
                    }
                    db.SaveChanges();
                    scope.Complete();

                    Session["cashBill"] = null;
                    Session["cashdetails"] = null;
                    Session["total"] = null;
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            return View("Create");
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

        public ActionResult Print(int id)
        {
            CashBill cashbill = db.CashBills.Find(id);
            CashBillDetail d = db.CashBillDetails.Find(id);
            
            if (cashbill == null)
            {
                return HttpNotFound();
            }

            return View(cashbill);           
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
