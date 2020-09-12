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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        public ActionResult Logout() 
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User"); 
        }
    }
}