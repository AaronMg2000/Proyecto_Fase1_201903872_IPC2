using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoIPC22011903872.Controllers
{
    public class PrincipalController : Controller
    {

        // GET: Principal
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || this.Session["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.user = this.Session["user"];
            return View();
        }

        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            Session.Remove("user");
            return RedirectToAction("Login", "User"); 
        }
    }
}