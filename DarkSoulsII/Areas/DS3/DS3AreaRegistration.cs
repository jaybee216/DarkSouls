using System.Web.Mvc;

namespace DarkSoulsII.Areas.DS3
{
    public class DS3AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DS3";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DS3_default",
                "DS3/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "DarkSoulsII.Areas.DS3.Controllers" }
            );
        }
    }
}