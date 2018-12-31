using System;
using System.Linq;
using DienMayQT.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using DienMayQT.Models;
using System.Collections.Generic;
using Moq;
using System.Web;
using System.Web.Routing;
using System.Transactions;
using DienMayQT.Areas.Admin.Controllers;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new HomeController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["UserName"]).Returns("abc");

            var result = controller.Index() as ViewResult;
            var db = new DmQT09Entities();

            //Assert.IsNotNull(result.ViewBag.Message);
            Assert.IsInstanceOfType(result.Model, typeof(List<Product>));
            Assert.AreEqual(db.Products.Count(), (result.Model as List<Product>).Count);
        }
        [TestMethod]
        public void getProductType()
        {
            var controller = new ProductTypesController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<ProductType>));
        }
        [TestMethod]
        public void CreateProductTest()
        {
            var controller = new ProductAdminController();
            var db = new DmQT09Entities();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            context.Setup(c => c.Request).Returns(request.Object);
            request.Setup(r => r.Files).Returns(files.Object);
            files.Setup(f => f["ImageFile"]).Returns(file.Object);
            file.Setup(f => f.ContentLength).Returns(1);
            file.Setup(c => c.FileName).Returns("image.png");
            context.Setup(c => c.Server.MapPath("/Images/")).Returns("/Images/");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            using (var scope = new TransactionScope())
            {

                var model = new Product();
                model.ProductTypeID = db.ProductTypes.First().ID;
                model.ProductName = "ProductName";
                model.OriginPrice = 123;
                model.SalePrice = 456;
                model.InstallmentPrice = 789;
                model.Quantity = 10;

                var result0 = controller.Create(model) as ViewResult;
                Assert.IsNotNull(result0);
            }
        }

        [TestMethod]
        public void selectTest()
        {
            var controller = new ProductAdminController();
            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData["ProductTypeID"], typeof(SelectList));
        }
        [TestMethod]
        public void EditProductTest()
        {
            var controller = new ProductAdminController();
            var db = new DmQT09Entities();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            context.Setup(c => c.Request).Returns(request.Object);
            request.Setup(r => r.Files).Returns(files.Object);
            files.Setup(f => f["ImageFile"]).Returns(file.Object);
            file.Setup(f => f.ContentLength).Returns(1);
            file.Setup(c => c.FileName).Returns("image.png");
            context.Setup(c => c.Server.MapPath("/Images/")).Returns("/Images/");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var model = db.Products.AsNoTracking().First();

            using (var scope = new TransactionScope())
            {
                model.Quantity = 10;
                model.SalePrice = 20000;
                model.OriginPrice = 10000;
                model.InstallmentPrice = 300000;
                model.ProductName = "testabcdetestabcde";
                model.ProductTypeID = 2;

                var result = controller.Edit(model) as RedirectToRouteResult;
                Assert.IsNotNull(result);
            }
        }

    }
   }
