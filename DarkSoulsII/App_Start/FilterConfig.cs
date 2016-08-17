using DarkSoulsII.Attributes;
using log4net;
using System.Web.Mvc;

namespace DarkSoulsII
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, ILog logger)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionLoggingFilter(logger));
        }
    }
}
