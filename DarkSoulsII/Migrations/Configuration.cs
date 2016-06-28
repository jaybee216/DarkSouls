namespace DarkSoulsII.Migrations
{
    using System.Data.Entity.Migrations;
    using Microsoft.AspNet.Identity;
    using DataAccess;
    //using Models;
    using Model;
    using Model.DS2;
    using System.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.DarkSoulsIIContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DataAccess.DarkSoulsIIContext";
        }

        protected override void Seed(DarkSoulsIIContext context)
        {
            InitializeIdentityForEF(context);

            base.Seed(context);
        }

        public static void InitializeIdentityForEF(DarkSoulsIIContext db)
        {
            //OWIN is not accessible when updating through the Package Manager Console, so we need to instantiate these explicitly
            //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            string name = ConfigurationManager.AppSettings["defaultAdminUserName"];
            string password = ConfigurationManager.AppSettings["defaultAdminPassword"];
            string roleName = ConfigurationManager.AppSettings["defaultAdminRoleName"];

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}
