using System.Web;
using System.Web.Mvc;

namespace TranVanDuc_2122110512
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
