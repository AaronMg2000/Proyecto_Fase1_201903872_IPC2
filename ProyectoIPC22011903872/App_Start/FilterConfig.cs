using System.Web;
using System.Web.Mvc;

namespace ProyectoIPC22011903872
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
