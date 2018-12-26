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
           
                return View();
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {

            using (DmQT09Entities db = new DmQT09Entities())
            {
                var obj = db.Users.Where(a => a.username.Equals(objUser.username) && a.password.Equals(objUser.password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserType"] = obj.UserType.UserTypeCode.ToString();
                    Session["Username"] = obj.fullname.ToString();
                    return RedirectToAction("Permision", "Permision");
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