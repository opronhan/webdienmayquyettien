using DienMayQT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DienMayQT.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        //
        // GET: /Admin/Login/
        public ActionResult Login()
        {

            if (Session["Username"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "ProductAdmin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account objUser)
        {

            using (DmQT09Entities db = new DmQT09Entities())
            {
                var obj = db.Accounts.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["Username"] = obj.Fullname.ToString();
                    return RedirectToAction("Index","ProductAdmin");
                }
            }

            return View(objUser);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login");
        }
	}
}