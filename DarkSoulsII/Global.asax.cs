using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DarkSoulsII.Infrastructure;
using log4net;

namespace DarkSoulsII
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            var logger = LogManager.GetLogger("GlobalLogger");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, logger);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var primeDel = new PrimeCacheDelegate(PrimeCache);
            primeDel.BeginInvoke(null, null);
        }

        protected delegate void PrimeCacheDelegate();

        private void PrimeCache()
        {
            IoC.Resolve<ICache>().Prime();
        }
    }
}
