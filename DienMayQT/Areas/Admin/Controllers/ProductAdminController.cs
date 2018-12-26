using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using DienMayQT.Models;
using System.Data.Entity.Infrastructure;



namespace DienMayQT.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        DmQT09Entities db = new DmQT09Entities();
        //
        // GET: /Admin/ProductAdmin/
        public ActionResult Index()
        {
            var product = db.Products.OrderByDescending(x => x.ID).ToList();
            if (Session["Username"] != null && Session["NVKDPer"] != null)
            {   
                return View(product);
            }
            else
            {
                return RedirectToAction("Login","LoginAdmin");
            }
        }

        public FileResult Details(string imageName)
        {
            var path = Server.MapPath("~/Images/" + imageName);
            return File(path, "images");
        }

        // GET: /DanhSachSanPham/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes.OrderByDescending(x => x.ID).ToList(), "ID", "ProductTypeName");
            return View();
        }

        // POST: /DanhSachSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            CheckBangSanPham(model);
            if (model.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Chưa có hình sản phẩm!");
            }
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.Products.Add(model);

                    if (model.ImageFile != null)
                    {
                        var imageName = System.IO.Path.GetFileName(model.ImageFile.FileName);
                        var path = Server.MapPath("~/Images");
                        path = System.IO.Path.Combine(path, imageName);
                        model.ImageFile.SaveAs(path);
                        model.Avatar = imageName;
                    }
                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Index");
                    
                }
            }

            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", model.ProductTypeID);
            return View(model);
        }

        private void CheckBangSanPham(Product model)
        {
            if (model.OriginPrice < 0)
                ModelState.AddModelError("OriginPrice", "Gia goc phai lon hon 0");
            if (model.OriginPrice > model.SalePrice)
                ModelState.AddModelError("SalePrice", "Gia ban phai lon hon gia goc");
            if (model.OriginPrice > model.InstallmentPrice)
                ModelState.AddModelError("InstallmentPrice", "Gia gop phai lon hon gia goc");
            if (model.Quantity < 0)
                ModelState.AddModelError("SoLuongTon", "So luong ton phai lon hon 0");
        }

        // GET: /DanhSachSanPham/Edit/5
        public ActionResult Edit(int id)
        {
            Product model = db.Products.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", model.ProductTypeID);
            return View(model);
        }

        // POST: /DanhSachSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            CheckBangSanPham(model);
            if (model.ImageFile == null)
            {
                Product thisProduct = db.Products.Where(p => p.ID == model.ID).FirstOrDefault();
                model.Avatar = thisProduct.Avatar;
            }
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    Product thisProduct = db.Products.Where(p => p.ID == model.ID).FirstOrDefault();
                   
                    if (model.ImageFile != null)
                    {
                        var imageName = System.IO.Path.GetFileName(model.ImageFile.FileName);
                        var path = Server.MapPath("~/Images");
                        path = System.IO.Path.Combine(path, imageName);
                        model.ImageFile.SaveAs(path);
                        model.Avatar = imageName;
                    }
                    db.Entry(thisProduct).CurrentValues.SetValues(model);  
                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }
           
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", model.ProductTypeID);
            return View(model);
        }
        // GET: /DanhSachSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product bangsanpham = db.Products.Find(id);
            if (bangsanpham == null)
            {
                return HttpNotFound();
            }
            return View(bangsanpham);
        }

        // POST: /DanhSachSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product bangsanpham = db.Products.Find(id);
            db.Products.Remove(bangsanpham);
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