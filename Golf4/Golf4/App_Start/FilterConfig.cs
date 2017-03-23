using System.Web;
using System.Web.Mvc;

namespace Golf4
{
    public class FilterConfig
    {
        // Ett nytt filter som sköter AUTH
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
        }
    }
}
