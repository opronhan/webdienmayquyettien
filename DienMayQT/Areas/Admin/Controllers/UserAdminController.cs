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
    public class UserAdminController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: /Admin/User/
        public ActionResult Index()
        {
            var user = db.Users.OrderByDescending(x => x.ID).ToList();
            if (Session["Username"] != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "LoginAdmin");
            }
        }

        public ActionResult Create()
        {
            ViewBag.UserTypeID = new SelectList(db.UserTypes.OrderByDescending(x => x.ID).ToList(), "ID", "UserTypeName");
            return View();
        }

        // POST: /DanhSachSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model)
        {
            checkUsername(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.Users.Add(model);
                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "ID", "UserTypeName", model.UserTypeID);
            return View(model);
        }

        // GET: /DanhSachSanPham/Edit/5
        public ActionResult Edit(int id)
        {
            User model = db.Users.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "ID", "UserTypeName", model.UserTypeID);
            return View(model);
        }

        // POST: /DanhSachSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            checkUsername(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    User thisUser = db.Users.Where(p => p.ID == model.ID).FirstOrDefault();
                    db.Entry(thisUser).CurrentValues.SetValues(model);
                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "ID", "UserTypeName", model.UserTypeID);
            return View(model);
        }
        private void checkUsername(User model)
        {
            var user = db.Users.OrderByDescending(x => x.ID).ToList();
            foreach (var item in user)
            {
                if (item.username == model.username && item.ID != model.ID)
                {
                    ModelState.AddModelError("username", "Tên tài khoản này đã tồn tại !");
                }
            }
        }
        // GET: /Admin/User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
