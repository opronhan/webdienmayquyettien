using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DienMayQT.Areas.Admin.Controllers
{
    public class PermisionController : Controller
    {
        //
        // GET: /Admin/Permision/
        public ActionResult Permision()
        {
            if (Session["UserType"].ToString() == "NVKD")
            {
                Session["NVKDPer"] = "true";
                Session["KTPer"] = null;
                return RedirectToAction("Index", "ProductAdmin");
            }
            Session["NVKDPer"] = null;
            Session["KTPer"] = "true";
            return RedirectToAction("Index", "UserAdmin");

        }
	}
}