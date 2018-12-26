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
    public class AdsAdminController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: /Admin/AdsAdmin/
        public ActionResult Index()
        {
            var ads = db.slideAds.OrderByDescending(x => x.ID).ToList();
            if (Session["Username"] != null)
            {
                return View(ads);
            }
            else
            {
                return RedirectToAction("Login", "LoginAdmin");
            }
        }

        // GET: /Admin/AdsAdmin/Details/5
        public FileResult Details(string imageName)
        {
            var path = Server.MapPath("~/ImgAds/" + imageName);
            return File(path, "imgads");
        }

        // GET: /Admin/AdsAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/AdsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(slideAd model)
        {
            if (model.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Chưa có hình sản phẩm!");
            }
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.slideAds.Add(model);

                    if (model.ImageFile != null)
                    {
                        var imageName = System.IO.Path.GetFileName(model.ImageFile.FileName);
                        var path = Server.MapPath("~/ImgAds");
                        path = System.IO.Path.Combine(path, imageName);
                        model.ImageFile.SaveAs(path);
                        model.image = imageName;
                    }
                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }

            return View(model);
        }

        // GET: /Admin/AdsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            slideAd slidead = db.slideAds.Find(id);
            if (slidead == null)
            {
                return HttpNotFound();
            }
            return View(slidead);
        }

        // POST: /Admin/AdsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(slideAd model)
        {
            if (model.ImageFile == null)
            {
                slideAd thisAds = db.slideAds.Where(p => p.ID == model.ID).FirstOrDefault();
                model.image = thisAds.image;
            }
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    slideAd thisAds = db.slideAds.Where(p => p.ID == model.ID).FirstOrDefault();

                    if (model.ImageFile != null)
                    {
                        var imageName = System.IO.Path.GetFileName(model.ImageFile.FileName);
                        var path = Server.MapPath("~/ImgAds");
                        path = System.IO.Path.Combine(path, imageName);
                        model.ImageFile.SaveAs(path);
                        model.image = imageName;
                    }
                    db.Entry(thisAds).CurrentValues.SetValues(model);
                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }
            return View(model);
        }

        // GET: /Admin/AdsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            slideAd slidead = db.slideAds.Find(id);
            if (slidead == null)
            {
                return HttpNotFound();
            }
            return View(slidead);
        }

        // POST: /Admin/AdsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            slideAd slidead = db.slideAds.Find(id);
            db.slideAds.Remove(slidead);
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
