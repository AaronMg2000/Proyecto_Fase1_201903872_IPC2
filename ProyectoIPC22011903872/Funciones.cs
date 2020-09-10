using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIPC22011903872
{
    public static class Funciones
    {
        public static string IsActive(this HtmlHelper html,string control, string action)
        {
            var routeData = html.ViewContext.RouteData;
            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];
            var returnActive = control == routeControl && action == routeAction;
            return returnActive ? "active" : "joo";
        }
    }
}