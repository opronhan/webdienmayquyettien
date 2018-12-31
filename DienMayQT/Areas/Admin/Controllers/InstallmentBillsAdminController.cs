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
    public class InstallmentBillsAdminController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: Admin/InstallmentBillsAdmin
        public ActionResult Index()
        {
            var installmentBills = db.InstallmentBills.OrderByDescending(x => x.ID).ToList();
            if (Session["Username"] != null && Session["NVKDPer"] != null)
            {
                return View(installmentBills);
            }
            else
            {
                return RedirectToAction("Login", "LoginAdmin");
            }
        }
        public int takenSession(int taken)
        {
            Session["Taken"] = taken;
            return (int)Session["Taken"];
        }
        // GET: Admin/InstallmentBillsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            if (installmentBill == null)
            {
                return HttpNotFound();
            }
            return View(installmentBill);
        }

        // GET: Admin/InstallmentBillsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList((from s in db.Customers select new { ID = s.ID, Info = s.CustomerCode +" - "+ s.CustomerName }), "ID", "Info",null);

            return View(Session["installBill"]);
        }

        // POST: Admin/InstallmentBillsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstallmentBill installmentBill)
        {
            if (ModelState.IsValid)
            {
                Session["installBill"] = installmentBill;
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", installmentBill.CustomerID);
            return View(installmentBill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2()
        {
            using (var scope = new TransactionScope())
                try
                {
                    var insBill = Session["installBill"] as InstallmentBill;
                    var insBillDetails = Session["installBillDetail"] as List<InstallmentBillDetail>;
                    insBill.Date = DateTime.Now;
                    insBill.GrandTotal = (int)Session["total"];
                    insBill.Taken = (int)Session["Taken"];
                    insBill.Remain = ((int)Session["total"] - (int)Session["Taken"]);

                    db.InstallmentBills.Add(insBill);
                    db.SaveChanges();

                    foreach (var details in insBillDetails)
                    {
                        details.BillID = insBill.ID;
                        details.Product = null;
                        db.InstallmentBillDetails.Add(details);
                    }
                    db.SaveChanges();
                    scope.Complete();

                    Session["installBill"] = null;
                    Session["installBillDetail"] = null;
                    Session["total"] = null;
                    Session["Taken"] = null;
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            return View("Create");
        }

        // GET: Admin/InstallmentBillsAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            Session["installBill"] = installmentBill;
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", installmentBill.CustomerID);
            return View(installmentBill);
        }

        // POST: Admin/InstallmentBillsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstallmentBill installmentBill)
        {
            if (ModelState.IsValid)
            {
                installmentBill.Date = DateTime.Now;
                db.Entry(installmentBill).State = EntityState.Modified;
                Session["installBill"] = installmentBill;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", installmentBill.CustomerID);
            return View(installmentBill);
        }

       

        // GET: Admin/InstallmentBillsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            if (installmentBill == null)
            {
                return HttpNotFound();
            }
            return View(installmentBill);
        }

        // POST: Admin/InstallmentBillsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            db.InstallmentBills.Remove(installmentBill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Print(int id)
        {
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);

            if (installmentBill == null)
            {
                return HttpNotFound();
            }

            return View(installmentBill);
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
