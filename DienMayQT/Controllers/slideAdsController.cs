using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DienMayQT.Models;

namespace DienMayQT.Controllers
{
    public class slideAdsController : Controller
    {
        private DmQT09Entities db = new DmQT09Entities();

        // GET: slideAds
        public ActionResult Index()
        {
            return View(db.slideAds.ToList());
        }

        // GET: slideAds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            slideAd slideAd = db.slideAds.Find(id);
            if (slideAd == null)
            {
                return HttpNotFound();
            }
            return View(slideAd);
        }

        // GET: slideAds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: slideAds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,image,status,nameAds,content")] slideAd slideAd)
        {
            if (ModelState.IsValid)
            {
                db.slideAds.Add(slideAd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slideAd);
        }

        // GET: slideAds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            slideAd slideAd = db.slideAds.Find(id);
            if (slideAd == null)
            {
                return HttpNotFound();
            }
            return View(slideAd);
        }

        // POST: slideAds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,image,status,nameAds,content")] slideAd slideAd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(slideAd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slideAd);
        }

        // GET: slideAds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            slideAd slideAd = db.slideAds.Find(id);
            if (slideAd == null)
            {
                return HttpNotFound();
            }
            return View(slideAd);
        }

        // POST: slideAds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            slideAd slideAd = db.slideAds.Find(id);
            db.slideAds.Remove(slideAd);
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
